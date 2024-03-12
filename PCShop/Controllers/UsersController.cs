using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCShop.Entities;
using PCShop.Models;

namespace PCShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public UsersController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
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
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("All", "Products");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid
                || await userManager.FindByNameAsync(registerVM.Username) is not null)
            {
                return View(registerVM);
            }

            var user = new User()
            {
                FirstName = registerVM.Username,
                LastName = registerVM.LastName,
                Email = registerVM.Email,
                UserName = registerVM.Username,
                RegisterDate = DateTime.UtcNow,
                IsAdministrator = false,
            };

            var result = await userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                return View();
            }

            userManager.AddToRoleAsync(user, "Client").Wait();
            await signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("All", "Products");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            return View(new UserVM()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Age = user.Age < 14 || user.Age > 120 ? null : user.Age,
                Username = user.UserName,
                RegisterDate = user.RegisterDate,
                PhoneNumber = user.PhoneNumber,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserVM userVM)
        {
            var user = await userManager.GetUserAsync(User);

            if (user is null)
            {
                return RedirectToAction(nameof(Logout));
            }

            user.FirstName = userVM.FirstName;
            user.LastName = userVM.LastName;
            user.Email = userVM.Email;
            user.Age = userVM.Age;
            user.PhoneNumber = userVM.PhoneNumber;

            await userManager.UpdateAsync(user);

            return View(userVM);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await userManager
                .ChangePasswordAsync(user, changePasswordVM.OldPassword, changePasswordVM.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await signInManager.RefreshSignInAsync(user);

            return RedirectToAction(nameof(Profile));
        }
    }
}
