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
    using ChooseAndBuy.Web.ViewModels.Orders;
    using ChooseNBuy.Controllers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : BaseController
    {
        private const decimal DeliveryFee = 3.90m;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICityService cityService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IAddressService addressService;
        private readonly IMapper mapper;

        public OrdersController(
            UserManager<ApplicationUser> userManager,
            ICityService cityService,
            IShoppingCartService shoppingCartService,
            IAddressService addressService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.cityService = cityService;
            this.shoppingCartService = shoppingCartService;
            this.addressService = addressService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var cities = this.cityService.GetAllCities();

            var userAddresses = this.addressService.GetAllUserAddresses(user.Id);
            var userAddressesViewModels = this.mapper.Map<List<AddressViewModel>>(userAddresses);

            var cartProducts = this.shoppingCartService.GetCartProductsByUserId(user.Id);
            var mappedCartProducts = this.mapper.Map<List<OrderProductViewModel>>(cartProducts);

            var totalPrice = mappedCartProducts.Sum(p => p.TotalPrice) + DeliveryFee;

            AddressCreateBindingModel addressBindingModel = new AddressCreateBindingModel
            {
                PhoneNumber = user.PhoneNumber,
                Cities = cities,
            };

            OrderBindingModel orderBindingModel = new OrderBindingModel
            {
                Products = mappedCartProducts,
                ApplicationUserId = user.Id,
                TotalPrice = totalPrice,
                DeliveryFee = DeliveryFee,
            };

            CheckoutViewModel model = new CheckoutViewModel
            {
                AddressCreate = addressBindingModel,
                UserAddresses = userAddressesViewModels,
                OrderCreate = orderBindingModel,
            };

            return this.View(model);
        }

        //public IActionResult Create (OrderBindingModel orderBindingModel)
        //{

        //}
    }
}