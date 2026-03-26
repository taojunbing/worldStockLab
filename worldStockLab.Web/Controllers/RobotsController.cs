using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace worldStockLab.Web.Controllers
{
    //robots.txt控制器，负责生成robots.txt文件，告诉搜索引擎哪些页面可以被爬取，哪些页面不可以被爬取
    public class RobotsController : Controller
    {
        [HttpGet("/robots.txt")]
        public IActionResult Index()
        {
           //自动获取当前网站域名
           var baseUrl = $"{Request.Scheme}://{Request.Host}";

            //构建robots.txt的内容，允许所有搜索引擎爬取网站的所有页面,StringBuilder是一个可变的字符串类，允许我们高效地构建字符串内容。我们使用它来逐行构建robots.txt文件的内容。
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *");
            sb.AppendLine("Allow: /");
            sb.AppendLine("");

            //告诉搜索引擎sitemap.xml文件的位置，方便搜索引擎更好地爬取网站内容
            sb.AppendLine($"Sitemap: {baseUrl}/sitemap.xml");

            //返回robots.txt内容，设置Content-Type为text/plain
            return Content(sb.ToString(), "text/plain");
        }
    }
}
