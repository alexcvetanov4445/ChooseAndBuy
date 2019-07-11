namespace ChooseAndBuy.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.shoppingCartService = shoppingCartService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            // TODO: think about using the cookie if the user is not logged in
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            var products = this.shoppingCartService.GetCartProductsByUserId(userId).ToList();

            var mappedProducts = this.mapper.Map<List<ShoppingCartProductViewModel>>(products);

            var model = new ShoppingCartViewModel
            {
                Products = mappedProducts,
                TotalPrice = mappedProducts.Sum(x => x.TotalPrice),
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Add([FromBody]ShoppingCartAddInputModel model)
        {
            // TODO: think about using the cookie if the user is not logged in
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            var isProductAdded = this.shoppingCartService.AddProductToCart(model.ProductId, userId, model.Quantity);

            return this.Json(isProductAdded);
        }

        [HttpPost]
        public IActionResult UpdateProductCount(ShoppingCartUpdateCountBindingModel model)
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            this.shoppingCartService.UpdateProductCount(model.ProductId, userId, model.Quantity);

            return this.RedirectToAction("Index");
        }

        public IActionResult Remove(string productId)
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            this.shoppingCartService.RemoveProductFromCart(productId, userId);

            return this.RedirectToAction("Index");
        }
    }
}