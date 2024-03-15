using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Initialize SignInManager and UserManager
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        public UsersController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        /// <summary>
        /// Register page
        /// </summary>
        /// <returns>Return register page</returns>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Login page
        /// </summary>
        /// <returns>Return login page</returns>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// User login
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns>Success: Redirect to all products action, Failed: return View</returns>
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

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="registerVM"></param>
        /// <returns>Success: Redirect to all products action, Failed: return View</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid
                || await userManager.FindByNameAsync(registerVM.Username) is not null
                || await userManager.FindByEmailAsync(registerVM.Email) is not null)
            {
                return View(registerVM);
            }

            if (registerVM.Password != registerVM.ConfirmPassword)
            {
                ModelState.AddModelError("PasswordConfirm", "The passwords don't match!");
                return View();
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

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>Return home page</returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Profile page
        /// </summary>
        /// <returns>Return profile page with model of userVM</returns>
        [Authorize]
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

        /// <summary>
        /// Edit my user data
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns>Success: Return profile page, Failed: redirect to logout</returns>
        [Authorize]
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

        /// <summary>
        /// Create employee page
        /// </summary>
        /// <returns>Return create employee page</returns>
        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create employee
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns>Success: redirect to AllEmployees, Failed: return View</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM userVM)
        {
            if (!ModelState.IsValid
                 || await userManager.FindByNameAsync(userVM.Username) is not null)
            {
                return View();
            }

            var user = new User()
            {
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
                Email = userVM.Email,
                UserName = userVM.Username,
                RegisterDate = DateTime.UtcNow,
                IsAdministrator = false,
            };

            var result = await userManager.CreateAsync(user, "employee123");

            if (!result.Succeeded)
            {
                return View();
            }

            userManager.AddToRoleAsync(user, "Employee").Wait();

            return RedirectToAction(nameof(AllEmployees));
        }

        /// <summary>
        /// Change password page
        /// </summary>
        /// <returns>Return change password page</returns>
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="changePasswordVM"></param>
        /// <returns>Success: redirect to profile, Failed: return View or NotFound</returns>
        [Authorize]
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

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>Return all employees page</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllEmployees()
        {
            var users = await userManager.GetUsersInRoleAsync("employee");

            return View(users.Select(x => new UserVM()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Username = x.UserName,
                Age = x.Age,
                RegisterDate = x.RegisterDate,
                PhoneNumber = x.PhoneNumber,
                IsAdministrator = x.IsAdministrator,
            }));
        }

        /// <summary>
        /// Get all clients
        /// </summary>
        /// <returns>return all clients page</returns>
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> AllClients()
        {
            var users = await userManager.GetUsersInRoleAsync("client");

            return View(users.Select(x => new UserVM()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Username = x.UserName,
                Age = x.Age,
                RegisterDate = x.RegisterDate,
                PhoneNumber = x.PhoneNumber,
                IsAdministrator = x.IsAdministrator,
            }));
        }

        /// <summary>
        /// Promote the employee
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Redirect to all employees</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Promote(string userId)
        {
            if (userId == null)
            {
                return RedirectToAction(nameof(AllEmployees));
            }
            var user = await userManager.FindByIdAsync(userId);

            if (user == null || await userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction(nameof(AllEmployees));
            }

            user.IsAdministrator = true;
            await userManager.UpdateAsync(user);
            await userManager.AddToRoleAsync(user, "Admin");

            return RedirectToAction(nameof(AllEmployees));
        }

        /// <summary>
        /// Demote the employee
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Redirect to all employees</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Demote(string userId)
        {
            if (userId == null)
            {
                return RedirectToAction(nameof(AllEmployees));
            }
            var user = await userManager.FindByIdAsync(userId);

            if (user == null || !await userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction(nameof(AllEmployees));
            }

            user.IsAdministrator = false;
            await userManager.UpdateAsync(user);
            await userManager.RemoveFromRoleAsync(user, "Admin");

            return RedirectToAction(nameof(AllEmployees));
        }
    }
}
