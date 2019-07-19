namespace ChooseAndBuy.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly UserManager<ApplicationUser> userManager;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            UserManager<ApplicationUser> userManager)
        {
            this.shoppingCartService = shoppingCartService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            // TODO: think about using the cookie if the user is not logged in
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            var products = await this.shoppingCartService.GetCartProductsByUserId(userId);

            // TODO: might be a problem with mapping - not this model
            var mappedProducts = AutoMapper.Mapper.Map<List<ShoppingCartProductViewModel>>(products);

            var model = new ShoppingCartViewModel
            {
                Products = mappedProducts,
                TotalPrice = mappedProducts.Sum(x => x.TotalPrice),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]ShoppingCartAddInputModel model)
        {
            // TODO: think about using the cookie if the user is not logged in
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            var result = await this.shoppingCartService.AddProductToCart(model.ProductId, userId, model.Quantity);

            return this.Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductCount(ShoppingCartUpdateCountBindingModel model)
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            await this.shoppingCartService.UpdateProductCount(model.ProductId, userId, model.Quantity);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string productId)
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            var result = await this.shoppingCartService.RemoveProductFromCart(productId, userId);

            return this.Json(result);
        }
    }
}
