namespace ChooseAndBuy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.ViewModels.Addresses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AddressesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAddressService addressService;
        private readonly IMapper mapper;

        public AddressesController(
            UserManager<ApplicationUser> userManager,
            IAddressService addressService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.addressService = addressService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddressCreateBindingModel addressCreate)
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            var address = this.mapper.Map<Address>(addressCreate);
            address.ApplicationUserId = userId;

            this.addressService.CreateAddress(address);

            return this.RedirectToAction("Index", "Orders");
        }
    }
}