namespace ChooseNBuy.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Web.Controllers;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return this.View();
        }

        [Route("Contact")]
        public IActionResult Contact()
        {
            return this.View();
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }
    }
}
