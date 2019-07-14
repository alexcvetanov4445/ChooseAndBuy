namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;

    using ChooseAndBuy.Data.Models;

    public interface IShoppingCartService
    {
        bool AddProductToCart(string productId, string userId, int quantity);

        bool UpdateProductCount(string productId, string userId, int quantity);

        bool RemoveProductFromCart(string productId, string userId);

        bool RemoveAllCartProducts(string userId);

        IEnumerable<ShoppingCartProduct> GetCartProductsByUserId(string userId);
    }
}
