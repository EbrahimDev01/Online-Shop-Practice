using Microsoft.AspNetCore.Mvc;

namespace MyEshop.Mvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
