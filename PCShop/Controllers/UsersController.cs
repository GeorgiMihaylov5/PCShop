using Microsoft.AspNetCore.Mvc;
using PCShop.Abstraction;
using PCShop.Models;

namespace PCShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJWTService _jwtServoce;

        public UsersController(IUserService userService, IJWTService jwtServoce)
        {
            _userService = userService;
            _jwtServoce = jwtServoce;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM loginVM)
        {
            HttpContext.Session.SetString("da", "da");
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM registerVM)
        {
            return RedirectToAction("All", "Prrducts");
        }
    }
}
