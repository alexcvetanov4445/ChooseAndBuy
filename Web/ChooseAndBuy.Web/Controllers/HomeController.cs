namespace ChooseNBuy.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Web.Controllers;
    using ChooseAndBuy.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            return this.View();
        }

        [Route("Contact")]
        public async Task<IActionResult> Contact()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
