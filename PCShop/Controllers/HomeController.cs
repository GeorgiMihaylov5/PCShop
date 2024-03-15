using Microsoft.AspNetCore.Mvc;

namespace PCShop.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Home page
        /// </summary>
        /// <returns>Return the view of home page</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// About us page
        /// </summary>
        /// <returns>Return the view of about us page</returns>
        public IActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        /// Contacts page
        /// </summary>
        /// <returns>Return the view of contacts page</returns>
        public IActionResult Contacts()
        {
            return View();
        }
    }
}