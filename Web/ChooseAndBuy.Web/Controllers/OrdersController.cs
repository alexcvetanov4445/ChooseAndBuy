namespace ChooseAndBuy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using System.Linq;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Data.Models.Enums;
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
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(
            UserManager<ApplicationUser> userManager,
            ICityService cityService,
            IShoppingCartService shoppingCartService,
            IAddressService addressService,
            IOrderService orderService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.cityService = cityService;
            this.shoppingCartService = shoppingCartService;
            this.addressService = addressService;
            this.orderService = orderService;
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

        [HttpPost]
        public IActionResult Create(OrderBindingModel orderCreate)
        {
            // maps the order and sets its remaining values manually
            // after that the shoppingCart is deleted
            var order = this.mapper.Map<Order>(orderCreate);

            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            order.Quantity = orderCreate.Products.Sum(op => op.Quantity);

            order.DispatchDate = order.DeliveryType == DeliveryType.Express ?
                DateTime.UtcNow.AddDays(2) : DateTime.UtcNow.AddDays(5);

            // creates the order
            this.orderService.CreateOrder(order);

            // add the order products to the order
            this.orderService.AddProductsToOrder(order.Id, orderCreate.Products);

            // remove the products from the shopping cart after the order is added
            this.shoppingCartService.RemoveAllCartProducts(order.ApplicationUserId);

            return this.RedirectToAction("Confirmation", new { orderId = order.Id });
        }

        public IActionResult Confirmation(string orderId)
        {
            var order = this.orderService.GetOrderById(orderId);

            ConfirmationViewModel model = new ConfirmationViewModel
            {
                ExpectedDelivery = order.DispatchDate.Value.ToShortDateString(),
                PaymentMethod = order.PaymentType.ToString(),
                QuantityProducts = order.Quantity,
                TotalPrice = order.TotalPrice,
                PhoneNumber = order.DeliveryAddress.PhoneNumber,
                ClientName = order.DeliveryAddress.FirstName + " " + order.DeliveryAddress.LastName,
                Address = order.DeliveryAddress.AddressText,
                City = order.DeliveryAddress.City.Name,
            };

            return this.View(model);
        }

        public IActionResult UserOrders()
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            var orders = this.orderService.GetAllUserOrders(userId);

            var mappedOrders = this.mapper.Map<List<OrderViewModel>>(orders);

            UserOrdersViewModel model = new UserOrdersViewModel
            {
                Orders = mappedOrders,
            };

            return this.View(model);
        }
    }
}