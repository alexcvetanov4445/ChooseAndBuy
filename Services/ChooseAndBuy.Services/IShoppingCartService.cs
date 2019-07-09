namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ChooseAndBuy.Data.Models;

    public interface IShoppingCartService
    {
        bool AddProductToCart(string productId, string userId, int quantity);

        bool UpdateProductCount(string productId, string userId, int quantity);

        bool RemoveProductFromCart(string productId, string userId);

        IEnumerable<ShoppingCartProduct> GetCartProductsByUserId(string userId);
    }
}
