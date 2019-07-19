using ChooseAndBuy.Web.ViewModels.Favorites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChooseAndBuy.Services
{
    public interface IFavoritesService
    {
        Task<bool> Add(string productId, string userId);

        Task<bool> Remove(string productId, string userId);

        Task<IEnumerable<FavoriteProductViewModel>> GetUserFavorites(string userId);
    }
}
