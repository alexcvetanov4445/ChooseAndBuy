namespace ChooseNBuy.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data.Models;
    using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Cart()
        {
            return this.View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Checkout()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult Confirmation()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.SignInManager.SignOutAsync();
            return this.RedirectToAction("index", "home");
        }
    }
}
