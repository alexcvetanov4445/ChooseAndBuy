namespace ChooseAndBuy.Web.Controllers
{
    using System.Threading.Tasks;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class FavoritesController : BaseController
    {
        private readonly IFavoritesService favoritesService;
        private readonly UserManager<ApplicationUser> userManager;

        public FavoritesController(
            IFavoritesService favoritesService,
            UserManager<ApplicationUser> userManager)
        {
            this.favoritesService = favoritesService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            var products = await this.favoritesService.GetUserFavorites(userId);

            return this.View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string productId)
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            var result = await this.favoritesService.Add(productId, userId);

            return this.Json(new { success = result });

            // true - product added
            // false - product already added
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string productId)
        {
            string userId = this.userManager.GetUserId(this.HttpContext.User);

            var result = await this.favoritesService.Remove(productId, userId);

            return this.Json(result);
        }
    }
}
