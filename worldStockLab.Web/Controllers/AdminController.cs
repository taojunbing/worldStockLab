using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using worldStockLab.Web.Data;
using worldStockLab.Web.Models;
using Microsoft.AspNetCore.Authorization;
using worldStockLab.Web.Utils;

namespace worldStockLab.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region 后台管理

        // 后台首页（增加分页基础）
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 20;

            var articles = await _context.Articles
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(articles);
        }

        // 打开创建页面
        public IActionResult Create()
        {
            return View();
        }

        // 创建文章（修复Slug重复 + 安全）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (!ModelState.IsValid)
                return View(article);

            article.CreatedAt = DateTime.UtcNow;

            // 生成Slug
            var slug = SlugHelper.GenerateSlug(article.Title);

            // 保证唯一
            var exists = await _context.Articles.AnyAsync(a => a.Slug == slug);
            if (exists)
            {
                slug += "-" + Guid.NewGuid().ToString().Substring(0, 6);
            }

            article.Slug = slug;

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "文章创建成功！";

            return RedirectToAction(nameof(Index));
        }

        // 打开编辑页面
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            return View(article);
        }

        // 提交编辑（防止Overposting）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Article article)
        {
            if (!ModelState.IsValid)
                return View(article);

            var original = await _context.Articles.FindAsync(article.Id);
            if (original == null) return NotFound();

            // 只更新允许字段
            original.Title = article.Title;
            original.Summary = article.Summary;
            original.Content = article.Content;
            original.Category = article.Category;

            // 如果标题变了，更新Slug
            if (original.Title != article.Title)
            {
                var newSlug = SlugHelper.GenerateSlug(article.Title);

                var exists = await _context.Articles
                    .AnyAsync(a => a.Slug == newSlug && a.Id != article.Id);

                if (exists)
                {
                    newSlug += "-" + Guid.NewGuid().ToString().Substring(0, 6);
                }

                original.Slug = newSlug;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "文章更新成功！";

            return RedirectToAction(nameof(Index));
        }

        // 删除（改为POST + 防CSRF）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "文章已删除！";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region 图片上传

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("未上传文件");

            // 限制大小（2MB）
            if (file.Length > 2 * 1024 * 1024)
                return BadRequest("文件过大（最大2MB）");

            // 限制类型
            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!allowed.Contains(extension))
                return BadRequest("不支持的文件类型");

            // 生成文件名
            var fileName = Guid.NewGuid().ToString() + extension;

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            // 保存文件
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 返回URL
            var imageUrl = Url.Content($"~/uploads/{fileName}");

            return Json(new { url = imageUrl });
        }

        #endregion
    }
}