namespace ChooseAndBuy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;

    public class CategoriesController : Controller
    {
        [Route("Products")]
        public IActionResult Products()
        {
            return this.View();
        }
    }
}
