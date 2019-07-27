namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChooseAndBuy.Web.ViewModels.Orders;
    using ChooseAndBuy.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Http;

    public interface IShoppingCartService
    {
        Task<bool> AddProductToCart(string productId, string userId, int quantity);

        Task<bool> AddProductToSessionCart(ISession session, string productId, int quantity);

        Task<bool> UpdateProductCount(string productId, string userId, int quantity);

        Task<bool> UpdateSessionProductCount(ISession session, string productId, int quantity);

        Task<bool> RemoveProductFromCart(string productId, string userId);

        Task<bool> RemoveProductFromSessionCart(ISession session, string productId);

        Task<bool> RemoveAllCartProducts(string userId);

        Task<IEnumerable<OrderProductViewModel>> GetCartProductsByUserId(string userId);

        Task<bool> TransferSessionCartToAccountCart(string username, ISession session);

        ShoppingCartViewModel GetOrCreateSessionCart(ISession session);
    }
}
