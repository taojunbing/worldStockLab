using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using worldStockLab.Web.Data;
using worldStockLab.Web.Models;
using worldStockLab.Web.Services;
namespace worldStockLab.Web.Controllers
{


    #region 新版
    public class HomeController : Controller
    {
        //注入数据库上下文
        private readonly ApplicationDbContext _context;
        //构造函数，接受数据库上下文作为参数
        private readonly FearGreedService _fear;

      
       
        
        private readonly FinnhubService _finnhub;
        public HomeController(ApplicationDbContext context, FinnhubService finnhub,FearGreedService fear)
        {
            _context = context;
            _finnhub = finnhub;
            _fear = fear;
        }

       

        public async Task<IActionResult> Index()
        {

            // 获取主要指数数据
            //var dow = await _finnhub.GetIndex("^DJI");
            //var nasdaq = await _finnhub.GetIndex("^IXIC");
            //var sp500 = await _finnhub.GetIndex("^GSPC");
            var dow = await _finnhub.GetIndex("DIA");
            var nasdaq = await _finnhub.GetIndex("QQQ");
            var sp500 = await _finnhub.GetIndex("SPY");

            // 获取恐惧与贪婪指数
            var fearIndex = await _fear.GetIndex();
            // 将数据存储在ViewBag中，传递给视图进行渲染
            ViewBag.FearIndex = fearIndex;
            // 将指数数据存储在ViewBag中，传递给视图进行渲染
            ViewBag.Dow = dow;
            ViewBag.Nasdaq = nasdaq;
            ViewBag.Sp500 = sp500;



            // 股票（热门）
            var symbols = new[] { "AAPL", "NVDA", "MSFT", "TSLA" };

            var stocks = await _context.StockPrices
                .Where(x => symbols.Contains(x.Symbol))
                .ToListAsync();
            
            
            ViewBag.Stocks = stocks;

            //股票涨跌幅榜
            var gainers = await _context.StockPrices
                .OrderByDescending(x => x.ChangePercent)
                .Take(5)
                .ToListAsync();
            var losers = await _context.StockPrices
                .OrderBy(x => x.ChangePercent)
                .Take(5)
                .ToListAsync();

            // 最新文章
            var articles = await _context.Articles
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .ToListAsync();

            // 热门股票（浏览量）
            var hotStocks = await _context.StockPrices
                .OrderByDescending(x => x.ViewCount)
                .Take(5)
                .ToListAsync();

            // 分类数据（首页核心模块）

            ViewBag.AIStocks = await _context.StockPrices
                .Where(x => x.Category == "AI")
                .OrderByDescending(x => x.ViewCount)
                .Take(5)
                .ToListAsync();

            ViewBag.ETFStocks = await _context.StockPrices
                .Where(x => x.Category == "ETF")
                .OrderByDescending(x => x.ViewCount)
                .Take(5)
                .ToListAsync();

            ViewBag.SemiStocks = await _context.StockPrices
                .Where(x => x.Category == "Semiconductor")
                .OrderByDescending(x => x.ViewCount)
                .Take(5)
                .ToListAsync();



            //将数据封装到视图模型中，传递给视图进行渲染
            var vm = new HomeViewModel
            {

                Stocks = stocks,

                Articles = articles,

                Gainers = gainers,
                Losers = losers,

                HotStocks = hotStocks
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //关于页面
        public IActionResult About()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
           {
               return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
           }
    }
    #endregion
}
