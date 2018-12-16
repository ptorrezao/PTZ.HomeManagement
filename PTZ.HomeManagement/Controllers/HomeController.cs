using Microsoft.AspNetCore.Mvc;

namespace PTZ.HomeManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}