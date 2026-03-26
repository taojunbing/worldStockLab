using Microsoft.AspNetCore.Mvc;
using worldStockLab.Web.Data;
using Microsoft.EntityFrameworkCore;
using worldStockLab.Web.Services;
using worldStockLab.Web.Models;
namespace worldStockLab.Web.Controllers
{
    public class StockController : Controller
    {

        //依赖注入（Dependency Injection）。
        private readonly FinnhubService _finnhub;
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context, FinnhubService finnhub)
        {
            _context = context;
            _finnhub = finnhub;
        }


        //股票详情页面，显示股票行情数据
        // 股票详情
        public async Task<IActionResult> Detail(string id)
        {


            var symbol = id.ToUpper(); // 将股票代码转换为大写，确保与Yahoo Finance API的要求一致
                                       // var stock = await _yahooFinanceService.GetQuote(symbol);
                                       //var stock = await _finnhub.GetQuote(symbol);  //Controller → API
            var stock = await _context.StockPrices.FirstOrDefaultAsync(x => x.Symbol == symbol); //Controller → DB

            if (stock == null)
                return NotFound();

            //每次访问股票详情页面时，增加该股票的浏览量
            stock.ViewCount += 1;

            //将股票代码传递给视图，以便在视图中显示股票代码和相关信息
            ViewBag.TvSymbol = $"NASDAQ:{symbol}";

            //将更新后的浏览量保存到数据库中
            await _context.SaveChangesAsync();

            //查询相关文章
            var articles = await _context.Articles
               .Where(x => x.Symbol != null && x.Symbol.ToUpper() == symbol)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .ToListAsync();

            //将相关文章传递给视图，以便在视图中显示与该股票相关的新闻或分析文章
            //ViewBag是一个动态对象，可以在控制器中存储任意类型的数据，并将其传递给视图进行渲染
            ViewBag.Articles = articles;

            // 热门股票（用于右侧Sidebar）
            var hotStocks = await _context.StockPrices
                .OrderByDescending(x => x.ViewCount) // 按浏览量排序（更真实）
                .Take(5)
                .ToListAsync();

            ViewBag.HotStocks = hotStocks;

            return View(stock);
        }

        //股票列表页面，显示用户关注的股票列表
        //URL:/stock
        public async Task<IActionResult> Index()
        {
            //从数据库读取所有股票行情
            //OrderBy 按股票代码排序
            var stocks = await _context.StockPrices.OrderBy(x => x.Symbol).ToListAsync(); //从数据库中获取所有股票价格记录，并将它们转换为一个列表

            //将股票列表传递给视图进行渲染
            return View(stocks);
        }

        //涨幅榜
        public async Task<IActionResult> TopGainers()
        {
            var stocks = await _context.StockPrices
                .OrderByDescending(x => x.ChangePercent)
                .Take(5)
                .ToListAsync();

            return View(stocks);

        }

        //跌幅榜
        public async Task<IActionResult> TopLosers()
        {
            var stocks = await _context.StockPrices
                .OrderBy(x => x.ChangePercent)
                .Take(5)
                .ToListAsync();
            return View(stocks);
        }
    }
}