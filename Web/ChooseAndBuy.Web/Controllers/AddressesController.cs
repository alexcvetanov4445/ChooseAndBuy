namespace ChooseAndBuy.Web.Controllers
{
    using System.Threading.Tasks;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.BindingModels.Addresses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AddressesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAddressService addressService;

        public AddressesController(
            UserManager<ApplicationUser> userManager,
            IAddressService addressService)
        {
            this.userManager = userManager;
            this.addressService = addressService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddressCreateBindingModel addressCreate)
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            await this.addressService.CreateAddress(addressCreate, userId);

            return this.RedirectToAction("Index", "Orders");
        }
    }
}