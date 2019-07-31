namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Favorites;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class FavoritesService : IFavoritesService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public FavoritesService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<bool> Add(string productId, string userId)
        {
            // Checks if product exists
            var productExists = await this.ProductExists(productId);

            // Checks if user exists
            var userExists = await this.UserExists(userId);

            if (!productExists || !userExists)
            {
                return false;
            }

            // Checks if product is already in favorites
            var product = await this.context
                .UsersFavoriteProducts
                .SingleOrDefaultAsync(p => p.ProductId == productId);

            // If product is not null its in favorites already
            if (product != null)
            {
                return false;
            }

            UserFavoriteProduct userFavorite = new UserFavoriteProduct
            {
                ProductId = productId,
                ApplicationUserId = userId,
            };

            await this.context.UsersFavoriteProducts.AddAsync(userFavorite);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<FavoriteProductViewModel>> GetUserFavorites(string userId)
        {
            // Checks if user exists
            var userExists = await this.UserExists(userId);

            if (!userExists)
            {
                return null;
            }

            var products = this.context
                .UsersFavoriteProducts
                .Include(x => x.Product)
                .Where(p => p.ApplicationUserId == userId);

            var result = AutoMapper.Mapper.Map<List<FavoriteProductViewModel>>(products);

            return result;
        }

        public async Task<bool> Remove(string productId, string userId)
        {
            // Checks if product exists
            var productExists = await this.ProductExists(productId);

            // Checks if user exists
            var userExists = await this.UserExists(userId);

            if (!productExists || !userExists)
            {
                return false;
            }

            var product = await this.context
                .UsersFavoriteProducts
                .SingleOrDefaultAsync(p => p.ProductId == productId);

            this.context.UsersFavoriteProducts.Remove(product);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        private async Task<bool> UserExists(string userId)
        {
            var doesExist = await this.userManager.FindByIdAsync(userId);

            if (doesExist != null)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> ProductExists(string productId)
        {
            var doesExist = await this.context
                .Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (doesExist != null)
            {
                return true;
            }

            return false;
        }
    }
}
