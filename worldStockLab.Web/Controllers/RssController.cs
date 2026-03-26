using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using worldStockLab.Web.Data;

namespace worldStockLab.Web.Controllers
{
    //Rss Feed控制器
    public class RssController : Controller
    {
        //注入数据库上下文
        private readonly ApplicationDbContext _context;

        //构造函数，接受数据库上下文作为参数
        public RssController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Rss地址
        [HttpGet("/rss")]
        public async Task<IActionResult> Index()
        {
            // 从数据库中获取最新的20篇文章
            var articles = await _context.Articles.OrderByDescending(a => a.CreatedAt).Take(20).ToListAsync();
            
           
            //构建RSS Feed的内容
            var sb = new StringBuilder();
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<rss version=\"2.0\">");
            sb.AppendLine("<channel>");
            sb.AppendLine($"<title>World Stock Lab RSS Feed</title>");
            sb.AppendLine($"<link>{baseUrl}</link>");
            sb.AppendLine($"<description>全球股票研究与投资博客</description>");
            //遍历每篇文章，添加到RSS Feed中
            foreach (var article in articles)
            {
                sb.AppendLine("<item>");
                sb.AppendLine($"<title>{article.Title}</title>");
                sb.AppendLine($"<link>{baseUrl}/blog/{article.Slug}</link>");
           
                sb.AppendLine($"<pubDate>{article.CreatedAt:R}</pubDate>"); //R格式表示RFC1123格式的日期字符串
                sb.AppendLine("</item>");
            }
            sb.AppendLine("</channel>");
            sb.AppendLine("</rss>");
            //返回RSS Feed内容，设置Content-Type为application/rss+xml
            return Content(sb.ToString(), "application/xml");
        }
    }
}
