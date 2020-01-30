using Microsoft.AspNetCore.Mvc;

namespace MineLib.Server.Heartbeat.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}