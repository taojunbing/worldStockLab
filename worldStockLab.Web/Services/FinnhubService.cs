using System.Text.Json;
using worldStockLab.Web.Models;

namespace worldStockLab.Web.Services
{
    //YahooFinanceService类负责与Yahoo Finance API进行交互，获取股票行情数据
    public class FinnhubService
    {
        //HttpClient实例，用于发送HTTP请求
        private readonly HttpClient _http;
        private readonly string _apiKey = "d6sjnahr01qj447c01tgd6sjnahr01qj447c01u0";

        //构造函数，接受HttpClient参数并赋值给私有字段
        public  FinnhubService(HttpClient http)
        {
            _http = http;
        }

        //GetQuote方法接受一个股票代码参数，构建Yahoo Finance API的URL，发送HTTP请求获取股票行情数据，并解析JSON响应返回一个StockQuote对象
        public async Task<StockQuote> GetQuote(string symbol)
        {
            symbol = symbol.ToUpper();
            var url =
                $"https://finnhub.io/api/v1/quote?symbol={symbol}&token={_apiKey}";

            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Finnhub API 调用失败");
            }

            var json = await response.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new StockQuote
            {
                Symbol = symbol,

                Price = root.TryGetProperty("c", out var c) ? c.GetDecimal() : 0,

                Change = root.TryGetProperty("d", out var d) ? d.GetDecimal() : 0,

                ChangePercent = root.TryGetProperty("dp", out var dp) ? dp.GetDecimal() : 0,

                Time = DateTime.Now
            };
        }

        //GetIndex方法接受一个股票代码参数，调用GetQuote方法获取股票行情数据，并将其转换为StockPrice对象返回
        public async Task<StockPrice> GetIndex(string symbol)
        {
            var quote = await GetQuote(symbol);

            return new StockPrice
            {
                Symbol = symbol,
                Price = quote.Price,
                Change = quote.Change,
                ChangePercent = quote.ChangePercent
            };
        }
    }
}
