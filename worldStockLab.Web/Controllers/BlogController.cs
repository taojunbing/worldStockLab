using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using worldStockLab.Web.Data;

namespace worldStockLab.Web.Controllers
{
    //博客控制器，负责处理博客相关的请求
    public class BlogController : Controller
    {
        //注入数据库上下文
        private readonly ApplicationDbContext _context;

        //构造函数，接受数据库上下文作为参数
        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        //博客首页，显示所有文章
        public async Task<IActionResult> Index()
        {
            // 从数据库读取所有文章
            var articles = await _context.Articles.OrderByDescending(a => a.CreatedAt).ToListAsync();

            return View(articles);
        }

        //文章详情页，显示单篇文章的内容
        public async Task<IActionResult> Detail(string slug)
        {
            // 根据文章ID从数据库读取文章详情
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Slug == slug); //await的意思是等待异步操作完成后再继续执行后面的代码。在这里，FirstOrDefaultAsync方法是一个异步方法，它会在数据库中查找符合条件的第一条记录，并返回该记录。如果没有找到符合条件的记录，则返回null。通过使用await，我们可以在等待数据库查询完成的同时，不阻塞当前线程，从而提高应用程序的性能和响应能力。
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        //标签页，显示带有特定标签的文章列表
        public async Task<IActionResult> Tag(string tag)
        {
            var articles = await _context.ArticleTags //从ArticleTags表中查询
                .Where(at => at.Tag.Slug == tag)   //根据标签的Slug过滤
                .Select(at => at.Article)      //选择关联的文章
                .ToListAsync();  //将结果转换为列表

            return View(articles);
        }
    }
}