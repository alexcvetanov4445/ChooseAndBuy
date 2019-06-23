namespace ChooseNBuy.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.BindingModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        public UsersController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }

        public UserManager<ApplicationUser> UserManager { get; }

        public SignInManager<ApplicationUser> SignInManager { get; }

        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.SignInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return this.RedirectToAction("Index", "Home");
            }

            this.ModelState.AddModelError(string.Empty, "The username or password was incorrect!");

            return this.View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
            };

            var result = await this.UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // isPersistent can be set to true to keep the user logged in after closing
                // await this.SignInManager.SignInAsync(user, isPersistent: false);
                return this.RedirectToAction("Login", "Users");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            return this.View();
        }

        public IActionResult Cart()
        {
            return this.View();
        }

        public IActionResult Checkout()
        {
            return this.View();
        }

        public IActionResult Confirmation()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.SignInManager.SignOutAsync();
            return this.RedirectToAction("index", "home");
        }
    }
}
