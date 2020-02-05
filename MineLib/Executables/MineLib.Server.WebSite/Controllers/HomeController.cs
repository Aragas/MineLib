using Microsoft.AspNetCore.Mvc;

namespace MineLib.Server.WebSite.Controllers
{
    public sealed class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}