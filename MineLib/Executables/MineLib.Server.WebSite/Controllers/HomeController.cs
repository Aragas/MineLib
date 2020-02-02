using Microsoft.AspNetCore.Mvc;

namespace MineLib.Server.Heartbeat.Controllers
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