using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using System.Text;
using worldStockLab.Web.Data;
using worldStockLab.Web.Models;

namespace worldStockLab.Web.Controllers
{
    public class SitemapController : Controller
    {
        //注入数据库上下文
        private readonly ApplicationDbContext _context;

        //构造函数，接受数据库上下文作为参数
        public SitemapController(ApplicationDbContext context)
        {
            _context = context;
        }

        //生成sitemap.xml
        [HttpGet("/sitemap.xml")]
        public async Task<IActionResult> Index()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var sb = new StringBuilder();

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            // =========================
            // 1️⃣ 首页
            // =========================
            sb.AppendLine("<url>");
            sb.AppendLine($"<loc>{baseUrl}/</loc>");
            sb.AppendLine("</url>");

            // =========================
            // 2️⃣ About页面
            // =========================
            sb.AppendLine("<url>");
            sb.AppendLine($"<loc>{baseUrl}/home/about</loc>");
            sb.AppendLine("</url>");

            // =========================
            // 3️⃣ 股票页面（🔥最重要）
            // =========================
            var stocks = await _context.StockPrices.ToListAsync();

            foreach (var stock in stocks)
            {
                sb.AppendLine("<url>");
                sb.AppendLine($"<loc>{baseUrl}/stock/detail/{stock.Symbol}</loc>");
                sb.AppendLine($"<lastmod>{stock.UpdatedAt:yyyy-MM-dd}</lastmod>");
                sb.AppendLine("</url>");
            }

            // =========================
            // 4️⃣ 博客文章
            // =========================
            var articles = await _context.Articles
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            foreach (var article in articles)
            {
                sb.AppendLine("<url>");
                sb.AppendLine($"<loc>{baseUrl}/blog/{article.Slug}</loc>");
                sb.AppendLine($"<lastmod>{article.CreatedAt:yyyy-MM-dd}</lastmod>");
                sb.AppendLine("</url>");
            }

            sb.AppendLine("</urlset>");

            return Content(sb.ToString(), "application/xml");
        }




    }
    }

