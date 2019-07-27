namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChooseAndBuy.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Http;

    public interface IShoppingCartService
    {
        Task<bool> AddProductToCart(string productId, string userId, int quantity);

        Task<bool> UpdateProductCount(string productId, string userId, int quantity);

        Task<bool> RemoveProductFromCart(string productId, string userId);

        Task<bool> RemoveAllCartProducts(string userId);

        Task<IEnumerable<OrderProductViewModel>> GetCartProductsByUserId(string userId);

        Task<bool> TransferSessionCartToAccountCart(string username, ISession session);
    }
}
