using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using worldStockLab.Web.Data;
using worldStockLab.Web.Models;

namespace worldStockLab.Web.Controllers
{
    public class SearchController : Controller
    {
        //注入数据库上下文
        private readonly ApplicationDbContext _context;

        //构造函数，接受数据库上下文作为参数
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        //搜索结果页，显示搜索结果
        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<Article>());
            }
            //从数据库中搜索标题或内容包含查询字符串的文章
            var articles = await _context.Articles
                .Where(a => a.Title.Contains(query) || a.Content.Contains(query) || a.Summary.Contains(query))
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
            ViewBag.Query = query; //将查询字符串传递给视图，以便在视图中显示搜索结果的标题或其他相关信息

            return View(articles);

        }

        //股票搜索接口
        //URL:/search/stocks?q=NV
        [HttpGet("/search/stocks")]
        public async Task<IActionResult> Stocks(string query) {
            if (string.IsNullOrEmpty(query)) return Json(new List<string>());
            query = query.ToUpper();

            //从数据库查找股票
            var symbols = await _context.StockPrices.Where(x=>x.Symbol.ToUpper().StartsWith(query))
                .Select(x=>x.Symbol)
                .Take(10)
                .ToListAsync();

            //返回JSON
            return Json(symbols);
        }
    }
}
