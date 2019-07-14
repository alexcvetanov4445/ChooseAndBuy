namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool AddProductToCart(string productId, string userId, int quantity)
        {
            // Calls a private method to do the job for getting the shopping cart id
            string shoppingCartId = this.GetOrCreateUserShoppingCart(userId);

            // Checks if a product is added to the cart and if so, applies just the quantity
            // Otherwise shopping cart product is created with the data
            bool isProductAdded = this.context
                .ShoppingCartProducts
                .Any(scp => scp.ProductId == productId && scp.ShoppingCartId == shoppingCartId);

            if (isProductAdded)
            {
                this.context
                    .ShoppingCartProducts
                    .SingleOrDefault(scp => scp.ProductId == productId && scp.ShoppingCartId == shoppingCartId)
                    .Quantity += quantity;

                this.context.SaveChanges();

                return true;
            }

            var shoppingCartProduct = new ShoppingCartProduct
            {
                ProductId = productId,
                ShoppingCartId = shoppingCartId,
                Quantity = quantity,
            };

            this.context.ShoppingCartProducts.Add(shoppingCartProduct);

            this.context.SaveChanges();

            return true;
        }

        public IEnumerable<ShoppingCartProduct> GetCartProductsByUserId(string userId)
        {
            // Checks if products exists in the user's cart.
            // If there are no products returns a new list empty list so no exceptions are thrown
            // Otherwise gets the products and returns them
            bool productsExist = this.context
                .ShoppingCarts
                .Any(u => u.ApplicationUserId == userId);

            if (!productsExist)
            {
                return new List<ShoppingCartProduct>();
            }

            var products = this.context
                .ShoppingCarts
                .Include(p => p.ShoppingCartProducts)
                .ThenInclude(x => x.Product)
                .SingleOrDefault(u => u.ApplicationUserId == userId)
                .ShoppingCartProducts;

            return products;
        }

        public bool RemoveAllCartProducts(string userId)
        {
            // gets the user's shopping cart id
            // then gets the products of the cart and removes them
            var shoppingCartId = this.GetOrCreateUserShoppingCart(userId);

            var products = this.context.ShoppingCartProducts.Where(x => x.ShoppingCartId == shoppingCartId);

            this.context.ShoppingCartProducts.RemoveRange(products);

            this.context.SaveChanges();

            return true;
        }

        public bool RemoveProductFromCart(string productId, string userId)
        {
            string shoppingCartId = this.GetOrCreateUserShoppingCart(userId);

            var product = this.context
                .ShoppingCartProducts
                .Where(x => x.ShoppingCartId == shoppingCartId && x.ProductId == productId)
                .SingleOrDefault();

            this.context.ShoppingCartProducts.Remove(product);
            this.context.SaveChanges();

            return true;
        }

        public bool UpdateProductCount(string productId, string userId, int quantity)
        {
            if (quantity <= 0)
            {
                return false;
            }

            var shoppingCartId = this.GetOrCreateUserShoppingCart(userId);

            var shoppingCartProduct = this.context
                 .ShoppingCartProducts
                 .SingleOrDefault(scp => scp.ProductId == productId && scp.ShoppingCartId == shoppingCartId);

            shoppingCartProduct.Quantity = quantity;
            this.context.SaveChanges();

            return true;
        }

        private string GetOrCreateUserShoppingCart(string userId)
        {
            // Creates shopping cart if the user dont have one and returns it's Id
            bool doesHaveShoppingCart = this.context
                .ShoppingCarts
                .Any(u => u.ApplicationUserId == userId);

            if (!doesHaveShoppingCart)
            {
                ShoppingCart cart = new ShoppingCart
                {
                    ApplicationUserId = userId,
                };

                this.context.ShoppingCarts.Add(cart);
                this.context.SaveChanges();
            }

            return this.context
                .ShoppingCarts
                .SingleOrDefault(sc => sc.ApplicationUserId == userId)
                .Id;
        }
    }
}
