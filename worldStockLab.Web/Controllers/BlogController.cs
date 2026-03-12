using Microsoft.AspNetCore.Mvc;

namespace worldStockLab.Web.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
