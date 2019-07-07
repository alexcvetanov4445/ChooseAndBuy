namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool AddProductToCart(string productId, string userId)
        {
            var shoppingCartId = this.context
                .ShoppingCarts
                .SingleOrDefault(u => u.ApplicationUserId == userId)
                .Id;

            bool isProductAdded = this.context
                .ShoppingCartProducts
                .Any(scp => scp.ProductId == productId);

            if (isProductAdded)
            {
                this.context
                    .ShoppingCartProducts
                    .SingleOrDefault(scp => scp.ProductId == productId)
                    .Quantity++;

                this.context.SaveChanges();

                return true;
            }

            // TODO: make the user select quantity of product in the product details view
            var shoppingCartProduct = new ShoppingCartProduct
            {
                ProductId = productId,
                ShoppingCartId = shoppingCartId,
                Quantity = 1,
            };

            this.context.ShoppingCartProducts.Add(shoppingCartProduct);

            this.context.SaveChanges();

            return true;
        }

        public IEnumerable<ShoppingCartProduct> GetCartProductsByUserId(string userId)
        {
            if (!this.context
                .ShoppingCarts
                .Any(u => u.ApplicationUserId == userId))
            {
                return new List<ShoppingCartProduct>();
            }

            var products = this.context
                .ShoppingCarts
                .SingleOrDefault(u => u.ApplicationUserId == userId)
                .ShoppingCartProducts;

            return products;
        }
    }
}
