namespace ChooseNBuy.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        public IActionResult Login()
        {
            return this.View();
        }

        public IActionResult Register()
        {
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
    }
}
