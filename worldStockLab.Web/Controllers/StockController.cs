using Microsoft.AspNetCore.Mvc;

namespace worldStockLab.Web.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
