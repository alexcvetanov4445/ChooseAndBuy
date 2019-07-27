namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Common;
    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Orders;
    using ChooseAndBuy.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using SessionExtensions = ChooseAndBuy.Services.Extensions.SessionExtensions;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext context;
        private readonly IProductService productService;

        public ShoppingCartService(
            ApplicationDbContext context,
            IProductService productService)
        {
            this.context = context;
            this.productService = productService;
        }

        public async Task<bool> AddProductToCart(string productId, string userId, int quantity)
        {
            // Calls a private method to do the job for getting the shopping cart id
            string shoppingCartId = await this.GetOrCreateUserShoppingCartById(userId);

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

        public async Task<bool> AddProductToSessionCart(ISession session, string productId, int quantity)
        {
            var sessionCart = this.GetOrCreateSessionCart(session);

            if (!sessionCart.Products.Any(x => x.Id == productId))
            {
                // if a product with provided id doesn't exist get the product and add it to the cart
                var product = await this.productService.GetProductForCart(productId);

                product.Quantity = quantity;

                product.TotalPrice = (double)(product.Price * product.Quantity);

                sessionCart.Products.Add(product);
            }
            else
            {
                // if a product exists change its quantity and total price
                var product = sessionCart.Products.SingleOrDefault(p => p.Id == productId);
                sessionCart.Products.Remove(product);

                product.Quantity += quantity;
                product.TotalPrice += (double)(product.Price * quantity);

                sessionCart.Products.Add(product);
            }

            sessionCart.TotalPrice = sessionCart.Products.Sum(p => p.TotalPrice);

            SessionExtensions.SetObjectAsJson(session, GlobalConstants.ShoppingCartSession, sessionCart);

            return true;
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
            var shoppingCartId = await this.GetOrCreateUserShoppingCartById(userId);

            var products = this.context.ShoppingCartProducts.Where(x => x.ShoppingCartId == shoppingCartId);

            this.context.ShoppingCartProducts.RemoveRange(products);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> RemoveProductFromCart(string productId, string userId)
        {
            string shoppingCartId = await this.GetOrCreateUserShoppingCartById(userId);

            var product = await this.context
                .ShoppingCartProducts
                .Where(x => x.ShoppingCartId == shoppingCartId && x.ProductId == productId)
                .SingleOrDefaultAsync();

            this.context.ShoppingCartProducts.Remove(product);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> RemoveProductFromSessionCart(ISession session, string productId)
        {
            var sessionCart = this.GetOrCreateSessionCart(session);

            var product = sessionCart.Products.SingleOrDefault(p => p.Id == productId);

            sessionCart.Products.Remove(product);

            sessionCart.TotalPrice = sessionCart.Products.Sum(p => p.TotalPrice);

            SessionExtensions.SetObjectAsJson(session, GlobalConstants.ShoppingCartSession, sessionCart);

            return true;
        }

        public async Task<bool> TransferSessionCartToAccountCart(string username, ISession session)
        {
            var sessionCart =
                SessionExtensions.GetObjectFromJson<ShoppingCartViewModel>(session, GlobalConstants.ShoppingCartSession);

            if (sessionCart == null)
            {
                // this happens if the cart is not being used as a session
                return false;
            }

            string shoppingCartId = await this.GetOrCreateUserShoppingCartByUsername(username);

            var products = AutoMapper.Mapper.Map<List<ShoppingCartProduct>>(sessionCart.Products);
            products.ForEach(p => p.ShoppingCartId = shoppingCartId);

            // Remove the user previously added products to the cart and add the new session ones.
            var productToRemove = await this.context
                .ShoppingCartProducts
                .Where(x => x.ShoppingCartId == shoppingCartId)
                .ToArrayAsync();

            this.context.ShoppingCartProducts.RemoveRange(productToRemove);

            await this.context.ShoppingCartProducts.AddRangeAsync(products);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateProductCount(string productId, string userId, int quantity)
        {
            if (quantity <= 0)
            {
                return false;
            }

            var shoppingCartId = await this.GetOrCreateUserShoppingCartById(userId);

            var shoppingCartProduct = await this.context
                 .ShoppingCartProducts
                 .SingleOrDefaultAsync(scp => scp.ProductId == productId && scp.ShoppingCartId == shoppingCartId);

            shoppingCartProduct.Quantity = quantity;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateSessionProductCount(ISession session, string productId, int quantity)
        {
            var sessionCart = this.GetOrCreateSessionCart(session);

            var product = sessionCart.Products.SingleOrDefault(p => p.Id == productId);
            sessionCart.Products.Remove(product);

            product.Quantity = quantity;

            product.TotalPrice = (double)(product.Price * product.Quantity);

            sessionCart.Products.Add(product);

            sessionCart.TotalPrice = sessionCart.Products.Sum(p => p.TotalPrice);

            SessionExtensions.SetObjectAsJson(session, GlobalConstants.ShoppingCartSession, sessionCart);

            return true;
        }

        public ShoppingCartViewModel GetOrCreateSessionCart(ISession session)
        {
            var sessionCart =
                SessionExtensions.GetObjectFromJson<ShoppingCartViewModel>(session, GlobalConstants.ShoppingCartSession);

            if (sessionCart == null)
            {
                sessionCart = new ShoppingCartViewModel();
            }

            if (sessionCart.Products == null)
            {
                sessionCart.Products = new List<ShoppingCartProductViewModel>();
            }

            return sessionCart;
        }

        private async Task<string> GetOrCreateUserShoppingCartById(string userId)
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

            var shoppingCartId = this.context
                .ShoppingCarts
                .SingleOrDefault(sc => sc.ApplicationUserId == userId)
                .Id;

            return shoppingCartId;
        }

        private async Task<string> GetOrCreateUserShoppingCartByUsername(string username)
        {
            // Creates shopping cart if the user dont have one and returns it's Id
            var user = await this.context.Users.SingleOrDefaultAsync(x => x.UserName == username);

            bool doesHaveShoppingCart = await this.context
                .ShoppingCarts
                .AnyAsync(u => u.ApplicationUserId == user.Id);

            if (!doesHaveShoppingCart)
            {
                ShoppingCart cart = new ShoppingCart
                {
                    ApplicationUserId = user.Id,
                };

                await this.context.ShoppingCarts.AddAsync(cart);
                await this.context.SaveChangesAsync();
            }

            var shoppingCartId = this.context
                .ShoppingCarts
                .SingleOrDefault(sc => sc.ApplicationUserId == user.Id)
                .Id;

            return shoppingCartId;
        }
    }
}
