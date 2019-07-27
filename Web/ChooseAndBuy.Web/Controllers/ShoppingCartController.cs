namespace ChooseAndBuy.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ChooseAndBuy.Common;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.Extensions;
    using ChooseAndBuy.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IProductService productService;
        private readonly UserManager<ApplicationUser> userManager;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            IProductService productService,
            UserManager<ApplicationUser> userManager)
        {
            this.shoppingCartService = shoppingCartService;
            this.productService = productService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                string userId = this.userManager.GetUserId(this.HttpContext.User);
                var products = await this.shoppingCartService.GetCartProductsByUserId(userId);

                var mappedProducts = AutoMapper.Mapper.Map<List<ShoppingCartProductViewModel>>(products);

                var model = new ShoppingCartViewModel
                {
                    Products = mappedProducts,
                    TotalPrice = mappedProducts.Sum(x => x.TotalPrice),
                };

                return this.View(model);
            }

            var sessionCart = SessionExtensions.GetObjectFromJson<ShoppingCartViewModel>(this.HttpContext.Session, GlobalConstants.ShoppingCartSession);

            if (sessionCart == null || sessionCart.Products == null)
            {
                sessionCart = new ShoppingCartViewModel
                {
                    Products = new List<ShoppingCartProductViewModel>(),
                };
            }

            return this.View(sessionCart);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]ShoppingCartAddInputModel model)
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);
            bool result = false;

            if (this.User.Identity.IsAuthenticated)
            {
                result = await this.shoppingCartService.AddProductToCart(model.ProductId, userId, model.Quantity);

                return this.Json(result);
            }

            var sessionCart =
                SessionExtensions.GetObjectFromJson<ShoppingCartViewModel>(this.HttpContext.Session, GlobalConstants.ShoppingCartSession);

            // if there isnt a cart, create one (make a service method for obtaining a session cart)
            if (sessionCart == null)
            {
                sessionCart = new ShoppingCartViewModel();
            }

            if (sessionCart.Products == null)
            {
                sessionCart.Products = new List<ShoppingCartProductViewModel>();
            }

            // if there isnt already the same product added, create one and add it
            if (!sessionCart.Products.Any(x => x.Id == model.ProductId))
            {
                var product = await this.productService.GetProductForCart(model.ProductId);

                product.Quantity = model.Quantity;

                product.TotalPrice = (double)(product.Price * product.Quantity);

                sessionCart.Products.Add(product);

                result = true;
            }
            else // if a product exists change its quantity and total price
            {
                var product = sessionCart.Products.SingleOrDefault(p => p.Id == model.ProductId);
                sessionCart.Products.Remove(product);

                product.Quantity += model.Quantity;
                product.TotalPrice += (double)(product.Price * model.Quantity);

                sessionCart.Products.Add(product);

                result = true;
            }

            sessionCart.TotalPrice = sessionCart.Products.Sum(p => p.TotalPrice);

            SessionExtensions.SetObjectAsJson(this.HttpContext.Session, GlobalConstants.ShoppingCartSession, sessionCart);

            return this.Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductCount(ShoppingCartUpdateCountBindingModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                string userId = this.userManager.GetUserId(this.HttpContext.User);

                await this.shoppingCartService.UpdateProductCount(model.ProductId, userId, model.Quantity);

                return this.RedirectToAction("Index");
            }

            // this prevents the product in the cart to be with 0 quantity.
            if (model.Quantity == 0)
            {
                return this.RedirectToAction("Index");
            }

            var sessionCart =
                SessionExtensions.GetObjectFromJson<ShoppingCartViewModel>(this.HttpContext.Session, GlobalConstants.ShoppingCartSession);

            var product = sessionCart.Products.SingleOrDefault(p => p.Id == model.ProductId);
            sessionCart.Products.Remove(product);

            product.Quantity = model.Quantity;

            product.TotalPrice = (double)(product.Price * product.Quantity);

            sessionCart.Products.Add(product);

            sessionCart.TotalPrice = sessionCart.Products.Sum(p => p.TotalPrice);

            SessionExtensions.SetObjectAsJson(this.HttpContext.Session, GlobalConstants.ShoppingCartSession, sessionCart);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string productId)
        {
            bool result = false;

            if (this.User.Identity.IsAuthenticated)
            {
                string userId = this.userManager.GetUserId(this.HttpContext.User);

                result = await this.shoppingCartService.RemoveProductFromCart(productId, userId);

                return this.Json(result);
            }

            var sessionCart =
                SessionExtensions.GetObjectFromJson<ShoppingCartViewModel>(this.HttpContext.Session, GlobalConstants.ShoppingCartSession);

            var product = sessionCart.Products.SingleOrDefault(p => p.Id == productId);

            sessionCart.Products.Remove(product);

            sessionCart.TotalPrice = sessionCart.Products.Sum(p => p.TotalPrice);

            SessionExtensions.SetObjectAsJson(this.HttpContext.Session, GlobalConstants.ShoppingCartSession, sessionCart);

            result = true;

            return this.Json(result);
        }
    }
}
