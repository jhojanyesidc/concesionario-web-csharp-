using Microsoft.AspNetCore.Mvc;

namespace ConcesionarioWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
