namespace ChooseAndBuy.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using AutoMapper;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IMapper mapper;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            IMapper mapper)
        {
            this.shoppingCartService = shoppingCartService;
            this.mapper = mapper;
        }

        public IActionResult Index(string userId)
        {
            var products = this.shoppingCartService.GetCartProductsByUserId(userId).ToList();

            var mappedProducts = this.mapper.Map<List<ShoppingCartProductViewModel>>(products);

            // mappedProducts.ForEach(x => x.TotalPrice = (double)(x.Quantity * x.Price));
            var model = new ShoppingCartViewModel
            {
                Products = mappedProducts,
                TotalPrice = mappedProducts.Sum(x => x.TotalPrice),
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Add([FromBody]string productId, [FromBody]int quantity)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var isProductAdded = this.shoppingCartService.AddProductToCart(productId, userId);

            if (!isProductAdded)
            {
                return this.RedirectToAction("Details", "Products", new { id = productId });
            }

            return this.RedirectToAction("Index", new { userId = userId });
        }
    }
}