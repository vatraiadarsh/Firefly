using Microsoft.AspNetCore.Mvc;

namespace WebServices.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
