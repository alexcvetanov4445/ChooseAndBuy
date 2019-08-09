namespace ChooseAndBuy.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Tests.Common;
    using ChooseAndBuy.Services.Tests.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ShoppingCartServiceTests
    {
        private async Task<string> SeedSingleUser(ApplicationDbContext context)
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "TestUserId",
                UserName = "TestUsername",
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user.Id;
        }

        private async Task<string> SeedSingleProduct(ApplicationDbContext context)
        {
            Product product = new Product
            {
                Id = "TestProductId",
                Name = "TestProductName",
            };

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            return product.Id;
        }

        private async Task<string> SeedUserWithShoppingCartProducts(ApplicationDbContext context)
        {
            // Seeding the user
            ApplicationUser user = new ApplicationUser
            {
                Id = "TestUserId",
                UserName = "TestUsername",
                ShoppingCart = new ShoppingCart
                {
                    Id = "TestShoppingCart",
                    ApplicationUserId = "TestUserId",
                },
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            // Seeding the products
            var products = new List<Product>()
            {
                new Product
                {
                    Id = "firstProduct",
                    Name = "firstName",
                },
                new Product
                {
                    Id = "secondProduct",
                    Name = "secondName",
                },
                new Product
                {
                    Id = "thirdProduct",
                    Name = "thirdName",
                },
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            // Seeding the Shopping Cart Products
            var shoppingCartProducts = new List<ShoppingCartProduct>()
            {
                new ShoppingCartProduct
                {
                    ProductId = "firstProduct",
                    ShoppingCartId = "TestShoppingCart",
                    Quantity = 1,
                },
                new ShoppingCartProduct
                {
                    ProductId = "secondProduct",
                    ShoppingCartId = "TestShoppingCart",
                    Quantity = 1,
                },
                new ShoppingCartProduct
                {
                    ProductId = "thirdProduct",
                    ShoppingCartId = "TestShoppingCart",
                    Quantity = 1,
                },
            };

            await context.ShoppingCartProducts.AddRangeAsync(shoppingCartProducts);
            await context.SaveChangesAsync();

            return user.Id;
        }

        public ShoppingCartServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task AddProductToCart_WithValidData_ShouldAddProductSuccessfully()
        {
            string onFalseErrorMessage = "The method returned false upon valid data input for creation.";
            string onNullMethodReturnErrorMessage = "The shopping cart was not created for the user before his first product.";
            string onNullDatabaseReturnErrorMessage = "The product was not added to the shopping cart.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Giving only valid data
            var productId = await this.SeedSingleProduct(context);
            var userId = await this.SeedSingleUser(context);
            var quantity = 2;

            var methodResult = await shoppingCartService.AddProductToCart(productId, userId, quantity);

            Assert.True(methodResult, onFalseErrorMessage);

            // Ensure that the shopping cart is created for the user before adding the first product
            var userShoppingCart = await context.ShoppingCarts.FirstOrDefaultAsync(sc => sc.ApplicationUserId == userId);

            AssertExtensions.NotNullWithMessage(
                userShoppingCart, onNullMethodReturnErrorMessage);

            // Ensure that the product has been added to the cart successfully
            var productInCart = userShoppingCart.ShoppingCartProducts
                .FirstOrDefault(p => p.ProductId == productId && p.Quantity == quantity);

            AssertExtensions.NotNullWithMessage(
                productInCart, onNullDatabaseReturnErrorMessage);
        }

        [Fact]
        public async Task AddProductToCart_WithInvalidUser_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true with invalid user.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Not seeding the user
            var productId = await this.SeedSingleProduct(context);
            var userId = "FakeUserId";
            var quantity = 2;

            var methodResult = await shoppingCartService.AddProductToCart(productId, userId, quantity);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task AddProductToCart_WithInvalidProduct_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true with invalid product.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Not seeding the product
            var productId = "FakeProductId";
            var userId = await this.SeedSingleUser(context);
            var quantity = 2;

            var methodResult = await shoppingCartService.AddProductToCart(productId, userId, quantity);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task AddProductToCart_WithInvalidQuantity_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true with invalid quantity for product.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Giving invalid quantity for product
            var productId = await this.SeedSingleProduct(context);
            var userId = await this.SeedSingleUser(context);
            var quantity = 0;

            var methodResult = await shoppingCartService.AddProductToCart(productId, userId, quantity);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task GetCartProductsByUserId_WithUserAndProducts_ShouldReturnCorrectProducts()
        {
            string onCountDifferenceErrorMessage = "The returned products count is not correct.";
            string onFalseErrorMessage = "The returned products are not correct.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with products in his cart
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            string[] expectedProducts = new string[] { "firstName", "secondName", "thirdName" };
            int expectedCount = expectedProducts.Count();

            var methodResult = await shoppingCartService.GetCartProductsByUserId(userId);

            AssertExtensions.EqualCountWithMessage(
                methodResult.Count(), 
                expectedCount, 
                onCountDifferenceErrorMessage);

            foreach (var product in methodResult)
            {
                Assert.True(expectedProducts.Contains(product.Name), onFalseErrorMessage);
            }
        }

        [Fact]
        public async Task GetCartProductsByUserId_WithNoUserProducts_ShouldReturnAnEmptyCollection()
        {
            string onEmptyCollectionErrorMessage = "The method did not return an empty collection upon no products.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding only User
            var userId = await this.SeedSingleUser(context);

            var methodResult = await shoppingCartService.GetCartProductsByUserId(userId);

            AssertExtensions.EmptyWithMessage(methodResult, onEmptyCollectionErrorMessage);
        }

        [Fact]
        public async Task GetCartProductsByUserId_WithInvalidUser_ShouldReturnAnEmptyCollection()
        {
            string onNonEmptyCollectionErrorMessage = "The method did not return an empty collection upon invalid user.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding only User
            var userId = "FakeUserId";

            var methodResult = await shoppingCartService.GetCartProductsByUserId(userId);

            AssertExtensions.EmptyWithMessage(methodResult, onNonEmptyCollectionErrorMessage);
        }

        [Fact]
        public async Task RemoveAllCartProducts_WithValidCartAndProducts_ShouldRemoveAllProductsSuccessfully()
        {
            string onFalseErrorMessage = "The method returned false with valid data.";
            string onCountDifferenceErrorMessage = "The shopping cart was not empty after the method call.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products
            var userId = await this.SeedUserWithShoppingCartProducts(context);

            var methodResult = await shoppingCartService.RemoveAllCartProducts(userId);

            Assert.True(methodResult, onFalseErrorMessage);

            var expectedCount = 0;
            var productsFromShoppingCart = context.ShoppingCarts
                .FirstOrDefault(sc => sc.ApplicationUserId == userId)
                .ShoppingCartProducts;

            AssertExtensions.EqualCountWithMessage(
                expectedCount,
                productsFromShoppingCart.Count,
                onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task RemoveAllCartProducts_WithInvalidUser_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true with invalid user Id.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding only User
            var userId = "FakeUserId";

            var methodResult = await shoppingCartService.RemoveAllCartProducts(userId);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task RemoveProductFromCart_WithValidUserAndProducts_ShouldRemoveProductSuccessfully()
        {
            string onFalseErrorMessage = "The method returned false with valid data.";
            string onCountDifferenceErrorMessage = "The product was not removed from the cart.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            var productId = "firstProduct";

            var methodResult = await shoppingCartService.RemoveProductFromCart(productId, userId);

            Assert.True(methodResult, onFalseErrorMessage);

            var expectedCount = 2;
            var productsFromShoppingCart = context.ShoppingCarts
                .FirstOrDefault(sc => sc.ApplicationUserId == userId)
                .ShoppingCartProducts;

            AssertExtensions.EqualCountWithMessage(
                expectedCount,
                productsFromShoppingCart.Count,
                onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task RemoveProductFromCart_WithInvalidUser_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true with invalid user Id.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding only Product
            var userId = "FakeUserId";
            var productId = await this.SeedSingleProduct(context);

            var methodResult = await shoppingCartService.RemoveProductFromCart(productId, userId);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task RemoveProductFromCart_WithInvalidProductId_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true with invalid product Id.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            var productId = "FakeProductId";

            var methodResult = await shoppingCartService.RemoveProductFromCart(productId, userId);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task UpdateProductCount_WithValidData_ShouldUpdateCountSuccessfully()
        {
            string onFalseErrorMessage = "The method returned false upon valid input.";
            string onCountDifferenceErrorMessage = "The product quantity was not changed.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products
            // Will take the first product from all and change the quantity
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            var productId = "firstProduct";
            // The current quantity is 1
            var newQuantity = 4;

            var methodResult = await shoppingCartService.UpdateProductCount(productId, userId, newQuantity);

            Assert.True(methodResult, onFalseErrorMessage);

            var cartProductFromDatabase = await context.ShoppingCartProducts
                .FirstOrDefaultAsync(op => op.ProductId == productId);

            AssertExtensions.EqualCountWithMessage(
                newQuantity,
                cartProductFromDatabase.Quantity,
                onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task UpdateProductCount_WithInvalidUser_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true upon invalid user Id.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products but using fake userId
            var userId = "FakeUserId";
            await this.SeedUserWithShoppingCartProducts(context);
            var productId = "firstProduct";
            var newQuantity = 4;

            var methodResult = await shoppingCartService.UpdateProductCount(productId, userId, newQuantity);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task UpdateProductCount_WithInvalidProduct_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true upon invalid product Id.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products but using fake productId
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            var productId = "FakeProductId";
            var newQuantity = 4;

            var methodResult = await shoppingCartService.UpdateProductCount(productId, userId, newQuantity);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task UpdateProductCount_WithInvalidQuantity_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true upon invalid product quantity.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            var productId = "FakeProductId";

            // Setting invalid quantity
            var newQuantity = 0;

            var methodResult = await shoppingCartService.UpdateProductCount(productId, userId, newQuantity);

            Assert.False(methodResult, onTrueErrorMessage);
        }
    }
}
