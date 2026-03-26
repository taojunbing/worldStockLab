using System.Text.Json;
using worldStockLab.Web.Models;

namespace worldStockLab.Web.Services
{
    //YahooFinanceService类负责与Yahoo Finance API进行交互，获取股票行情数据
    public class YahooFinanceService
    {
        //HttpClient实例，用于发送HTTP请求
        private readonly HttpClient _http;

        //构造函数，接受HttpClient参数并赋值给私有字段
        public YahooFinanceService(HttpClient http)
        {
            _http = http;
        }

        //GetQuote方法接受一个股票代码参数，构建Yahoo Finance API的URL，发送HTTP请求获取股票行情数据，并解析JSON响应返回一个StockQuote对象
        public async Task<StockQuote> GetQuote(string symbol)
        {
            // Yahoo Finance API
            var url =
                $"https://query1.finance.yahoo.com/v7/finance/quote?symbols={symbol}";

            //发送HTTP GET请求获取JSON响应
            //var json = await _http.GetStringAsync(url);
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // 模拟浏览器
            request.Headers.Add("User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            request.Headers.Add("Accept","application/json");

            request.Headers.Add("Accept-Language","en-US,en;q=0.9");

            request.Headers.Add("Referer","https://finance.yahoo.com/");

            var response = await _http.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);   // 加这一行测试

            //解析JSON响应，提取股票行情数据
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("quoteResponse", out var quoteResponse))
            {
                Console.WriteLine("JSON结构异常: 没有quoteResponse属性");  // 加这一行测试
                throw new Exception("Yahoo Finance API 返回结构异常");
            }

            var resultArray = quoteResponse.GetProperty("result");

            if (resultArray.GetArrayLength() == 0)
            {
                throw new Exception("股票代码不存在");
            }

            var result = resultArray[0];
           

            //创建并返回一个StockQuote对象，包含股票代码、价格、涨跌幅等信息
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Yahoo Finance API 调用失败");
            }
            else
            return new StockQuote
            {
                Symbol = symbol,
                Price = result.GetProperty("regularMarketPrice").GetDecimal(),
                Change = result.GetProperty("regularMarketChange").GetDecimal(),
                ChangePercent = result.GetProperty("regularMarketChangePercent").GetDecimal(),
                Time = DateTime.Now
            };
        }
    }
}
