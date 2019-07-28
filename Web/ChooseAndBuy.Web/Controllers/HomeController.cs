namespace ChooseNBuy.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.BindingModels.Home;
    using ChooseAndBuy.Web.Controllers;
    using ChooseAndBuy.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IContactService contactService;

        public HomeController(IContactService contactService)
        {
            this.contactService = contactService;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            return this.View();
        }

        [Route("Contact")]
        public async Task<IActionResult> Contact()
        {
            ContactBindingModel model = new ContactBindingModel();

            return this.View(model);
        }

        [Route("Privacy")]
        public async Task<IActionResult> Privacy()
        {
            return this.View();
        }

        [HttpPost]
        [Route("Contact")]
        public async Task<IActionResult> Contact(ContactBindingModel model)
        {
            await this.contactService.AddContactMessage(model);

            this.TempData["Success"] = $"Thank you for your feedback. Your message will be proccessed and replied if needed!";

            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
