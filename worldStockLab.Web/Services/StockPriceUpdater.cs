using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using worldStockLab.Web.Data;
using worldStockLab.Web.Models;

namespace worldStockLab.Web.Services
{
    /// <summary>
    /// 股票价格后台更新服务（生产级版本）
    /// </summary>
    public class StockPriceUpdater : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public StockPriceUpdater(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 持续循环（服务生命周期内一直运行）
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("🚀 开始新一轮股票更新...");

                try
                {
                    // 创建作用域（获取DbContext / Service）
                    using var scope = _scopeFactory.CreateScope();

                    var db = scope.ServiceProvider
                        .GetRequiredService<ApplicationDbContext>();

                    var api = scope.ServiceProvider
                        .GetRequiredService<FinnhubService>();

                    // =========================
                    // 1️⃣ 从数据库读取所有股票
                    // =========================
                    var stocks = await db.StockPrices
                        .AsTracking() // 确保可更新
                        .ToListAsync(stoppingToken);

                    foreach (var stock in stocks)
                    {
                        // 如果服务停止 → 立即退出
                        if (stoppingToken.IsCancellationRequested)
                            break;

                        try
                        {
                            // =========================
                            // 2️⃣ 标准化股票代码
                            // =========================
                            var symbol = stock.Symbol?.Trim().ToUpper();

                            if (string.IsNullOrEmpty(symbol))
                                continue;

                            Console.WriteLine($"📊 Updating {symbol}...");

                            // =========================
                            // 3️⃣ 调用 API
                            // =========================
                            var quote = await api.GetQuote(symbol);

                            // =========================
                            // 4️⃣ 数据校验（防止污染数据库）
                            // =========================
                            if (quote == null || quote.Price <= 0)
                            {
                                Console.WriteLine($"⚠️ {symbol} 数据无效，跳过");
                                continue;
                            }

                            // =========================
                            // 5️⃣ 更新基础行情
                            // =========================
                            stock.Symbol = symbol;

                            stock.Price = quote.Price;
                            stock.Change = quote.Change;
                            stock.ChangePercent = quote.ChangePercent;

                            // =========================
                            // 6️⃣ 安全更新金融数据（核心）
                            // =========================

                            // 成交量
                            if (quote.Volume > 0)
                            {
                                stock.Volume = quote.Volume;
                            }
                            else
                            {
                                Console.WriteLine($"⚠️ {symbol} Volume 无数据，保留旧值");
                            }

                            // 市值
                            if (quote.MarketCap > 0)
                            {
                                stock.MarketCap = quote.MarketCap;
                            }

                            // 52周最高
                            if (quote.High52Week > 0)
                            {
                                stock.High52Week = quote.High52Week;
                            }

                            // 52周最低
                            if (quote.Low52Week > 0)
                            {
                                stock.Low52Week = quote.Low52Week;
                            }

                            // 更新时间
                            stock.UpdatedAt = DateTime.UtcNow;

                            Console.WriteLine($"✅ {symbol} 更新成功: {stock.Price}");
                        }
                        catch (Exception ex)
                        {
                            // 单个股票失败不影响整体
                            Console.WriteLine($"❌ {stock.Symbol} 更新失败: {ex.Message}");
                        }

                        // =========================
                        // 7️⃣ API 限流控制（非常重要）
                        // =========================

                        int delay = stock.AssetType switch
                        {
                            "ETF" => 500,     // ETF更新更快
                            "Stock" => 800,   // 普通股票
                            "Crypto" => 300,  // 预留
                            _ => 800
                        };

                        await Task.Delay(delay, stoppingToken);
                    }

                    // =========================
                    // 8️⃣ 批量保存（性能关键）
                    // =========================
                    await db.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    // 整轮失败保护
                    Console.WriteLine($"🔥 本轮更新失败: {ex.Message}");
                }

                Console.WriteLine("⏱ 更新完成，等待下一轮...");

                // =========================
                // 9️⃣ 更新间隔（生产建议）
                // =========================
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}