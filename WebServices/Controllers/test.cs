using Microsoft.AspNetCore.Mvc;

namespace WebServices.Controllers
{
    public class test : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
