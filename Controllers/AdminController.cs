using Microsoft.AspNetCore.Mvc;

namespace EducareSA.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
