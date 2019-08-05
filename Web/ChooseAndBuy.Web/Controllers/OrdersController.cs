namespace ChooseAndBuy.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Common;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.BindingModels.Addresses;
    using ChooseAndBuy.Web.ViewModels.Administration.Orders;
    using ChooseAndBuy.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICityService cityService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IAddressService addressService;
        private readonly IOrderService orderService;

        public OrdersController(
            UserManager<ApplicationUser> userManager,
            ICityService cityService,
            IShoppingCartService shoppingCartService,
            IAddressService addressService,
            IOrderService orderService)
        {
            this.userManager = userManager;
            this.cityService = cityService;
            this.shoppingCartService = shoppingCartService;
            this.addressService = addressService;
            this.orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var cities = await this.cityService.GetAllCities();

            var userAddresses = await this.addressService.GetAllUserAddresses(user.Id);

            var cartProducts = await this.shoppingCartService.GetCartProductsByUserId(user.Id);

            // If there are no products in the cart the user will be redirected
            if (cartProducts.Count() == 0)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var totalPrice = cartProducts.Sum(p => p.TotalPrice) + GlobalConstants.DeliveryFee;

            AddressCreateBindingModel addressBindingModel = new AddressCreateBindingModel
            {
                PhoneNumber = user.PhoneNumber,
                Cities = cities,
            };

            OrderBindingModel orderBindingModel = new OrderBindingModel
            {
                Products = cartProducts.ToList(),
                ApplicationUserId = user.Id,
                TotalPrice = totalPrice,
                DeliveryFee = GlobalConstants.DeliveryFee,
            };

            CheckoutViewModel model = new CheckoutViewModel
            {
                AddressCreate = addressBindingModel,
                UserAddresses = userAddresses.ToList(),
                OrderCreate = orderBindingModel,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderBindingModel orderCreate)
        {

            // Validation
            if (!this.ModelState.IsValid)
            {

            }

            // Post Logic
            // creates the order and recieves it's id
            var orderId = await this.orderService.CreateOrder(orderCreate);

            // add the order products to the order
            await this.orderService.AddProductsToOrder(orderId, orderCreate.Products);

            // remove the products from the shopping cart after the order is added
            await this.shoppingCartService.RemoveAllCartProducts(orderCreate.ApplicationUserId);

            return this.RedirectToAction("Confirmation", new { orderId = orderId });
        }

        public async Task<IActionResult> Confirmation(string orderId)
        {
            var model = await this.orderService.GetConfirmationInfo(orderId);

            return this.View(model);
        }

        public async Task<IActionResult> UserOrders()
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            var ordersViewModel = await this.orderService.GetAllUserOrders(userId);

            UserOrderViewModel model = new UserOrderViewModel
            {
                Orders = ordersViewModel.ToList(),
            };

            return this.View(model);
        }
    }
}