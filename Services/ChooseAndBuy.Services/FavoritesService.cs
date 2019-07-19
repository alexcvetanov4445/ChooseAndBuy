namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Favorites;
    using Microsoft.EntityFrameworkCore;

    public class FavoritesService : IFavoritesService
    {
        private readonly ApplicationDbContext context;

        public FavoritesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Add(string productId, string userId)
        {
            var product = await this.context
                .UsersFavoriteProducts
                .SingleOrDefaultAsync(p => p.ProductId == productId);

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
            var products = this.context
                .UsersFavoriteProducts
                .Include(x => x.Product)
                .Where(p => p.ApplicationUserId == userId);

            var result = AutoMapper.Mapper.Map<List<FavoriteProductViewModel>>(products);

            return result;
        }

        public async Task<bool> Remove(string productId, string userId)
        {
            var product = await this.context
                .UsersFavoriteProducts
                .SingleOrDefaultAsync(p => p.ProductId == productId);

            this.context.UsersFavoriteProducts.Remove(product);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
