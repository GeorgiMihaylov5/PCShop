using Microsoft.AspNetCore.Mvc;

namespace PCShop.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
