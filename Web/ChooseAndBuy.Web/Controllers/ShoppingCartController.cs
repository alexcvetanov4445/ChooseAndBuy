namespace ChooseAndBuy.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.BindingModels.ShoppingCart;
    using ChooseAndBuy.Web.ViewModels.ShoppingCart;
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

            var sessionCart = this.shoppingCartService.GetOrCreateSessionCart(this.HttpContext.Session);

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

            result = await this.shoppingCartService
                .AddProductToSessionCart(this.HttpContext.Session, model.ProductId, model.Quantity);

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

            await this.shoppingCartService
                .UpdateSessionProductCount(this.HttpContext.Session, model.ProductId, model.Quantity);

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

            result = await this.shoppingCartService.RemoveProductFromSessionCart(this.HttpContext.Session, productId);

            return this.Json(result);
        }
    }
}
