namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddProductToCart(string productId, string userId, int quantity)
        {
            // Calls a private method to do the job for getting the shopping cart id
            string shoppingCartId = await this.GetOrCreateUserShoppingCart(userId);

            // Checks if a product is added to the cart and if so, applies just the quantity
            // Otherwise shopping cart product is created with the data
            bool isProductAdded = this.context
                .ShoppingCartProducts
                .Any(scp => scp.ProductId == productId && scp.ShoppingCartId == shoppingCartId);

            int result = 0;

            if (isProductAdded)
            {
                this.context
                    .ShoppingCartProducts
                    .SingleOrDefault(scp => scp.ProductId == productId && scp.ShoppingCartId == shoppingCartId)
                    .Quantity += quantity;

                result = await this.context.SaveChangesAsync();

                return result > 0;
            }

            var shoppingCartProduct = new ShoppingCartProduct
            {
                ProductId = productId,
                ShoppingCartId = shoppingCartId,
                Quantity = quantity,
            };

            await this.context.ShoppingCartProducts.AddAsync(shoppingCartProduct);

            result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<OrderProductViewModel>> GetCartProductsByUserId(string userId)
        {
            // Checks if products exists in the user's cart.
            // If there are no products returns a new list empty list so no exceptions are thrown
            // Otherwise gets the products and returns them
            bool productsExist = await this.context
                .ShoppingCarts
                .AnyAsync(u => u.ApplicationUserId == userId);

            if (!productsExist)
            {
                return new List<OrderProductViewModel>();
            }

            var products = this.context
                .ShoppingCarts
                .Include(p => p.ShoppingCartProducts)
                .ThenInclude(x => x.Product)
                .SingleOrDefault(u => u.ApplicationUserId == userId)
                .ShoppingCartProducts;

            var productsViewModel = AutoMapper.Mapper.Map<List<OrderProductViewModel>>(products);

            return productsViewModel;
        }

        public async Task<bool> RemoveAllCartProducts(string userId)
        {
            // gets the user's shopping cart id
            // then gets the products of the cart and removes them
            var shoppingCartId = await this.GetOrCreateUserShoppingCart(userId);

            var products = this.context.ShoppingCartProducts.Where(x => x.ShoppingCartId == shoppingCartId);

            this.context.ShoppingCartProducts.RemoveRange(products);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> RemoveProductFromCart(string productId, string userId)
        {
            string shoppingCartId = await this.GetOrCreateUserShoppingCart(userId);

            var product = await this.context
                .ShoppingCartProducts
                .Where(x => x.ShoppingCartId == shoppingCartId && x.ProductId == productId)
                .SingleOrDefaultAsync();

            this.context.ShoppingCartProducts.Remove(product);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateProductCount(string productId, string userId, int quantity)
        {
            if (quantity <= 0)
            {
                return false;
            }

            var shoppingCartId = await this.GetOrCreateUserShoppingCart(userId);

            var shoppingCartProduct = await this.context
                 .ShoppingCartProducts
                 .SingleOrDefaultAsync(scp => scp.ProductId == productId && scp.ShoppingCartId == shoppingCartId);

            shoppingCartProduct.Quantity = quantity;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        private async Task<string> GetOrCreateUserShoppingCart(string userId)
        {
            // Creates shopping cart if the user dont have one and returns it's Id
            bool doesHaveShoppingCart = await this.context
                .ShoppingCarts
                .AnyAsync(u => u.ApplicationUserId == userId);

            if (!doesHaveShoppingCart)
            {
                ShoppingCart cart = new ShoppingCart
                {
                    ApplicationUserId = userId,
                };

                await this.context.ShoppingCarts.AddAsync(cart);
                await this.context.SaveChangesAsync();
            }

            var shoppingCart = this.context
                .ShoppingCarts
                .SingleOrDefault(sc => sc.ApplicationUserId == userId)
                .Id;

            return shoppingCart;
        }
    }
}
