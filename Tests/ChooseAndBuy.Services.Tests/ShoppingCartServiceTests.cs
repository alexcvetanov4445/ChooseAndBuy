namespace ChooseAndBuy.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.BindingModels;
    using ChooseAndBuy.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ShoppingCartServiceTests
    {
        [Fact]
        public async Task AddProductToCart_WithValidData_ShouldAddProductSuccessfully()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Giving only valid data
            var productId = await this.SeedSingleProduct(context);
            var userId = await this.SeedSingleUser(context);
            var quantity = 2;

            var methodResult = await shoppingCartService.AddProductToCart(productId, userId, quantity);

            Assert.True(methodResult, "The method returned false upon valid data input for creation.");

            // Ensure that the shopping cart is created for the user before adding the first product
            var userShoppingCart = await context.ShoppingCarts.FirstOrDefaultAsync(sc => sc.ApplicationUserId == userId);

            AssertExtensions.NotNullWithMessage(
                userShoppingCart, "The shopping cart was not created for the user before his first product.");

            // Ensure that the product has been added to the cart successfully
            var productInCart = userShoppingCart.ShoppingCartProducts
                .FirstOrDefault(p => p.ProductId == productId && p.Quantity == quantity);

            AssertExtensions.NotNullWithMessage(productInCart, "The product was not added to the shopping cart.");
        }

        [Fact]
        public async Task AddProductToCart_WithInvalidUser_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Not seeding the user
            var productId = await this.SeedSingleProduct(context);
            var userId = "FakeUserId";
            var quantity = 2;

            var methodResult = await shoppingCartService.AddProductToCart(productId, userId, quantity);

            Assert.False(methodResult, "The method returned true with invalid user.");
        }

        [Fact]
        public async Task AddProductToCart_WithInvalidProduct_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Not seeding the product
            var productId = "FakeProductId";
            var userId = await this.SeedSingleUser(context);
            var quantity = 2;

            var methodResult = await shoppingCartService.AddProductToCart(productId, userId, quantity);

            Assert.False(methodResult, "The method returned true with invalid product.");
        }

        [Fact]
        public async Task AddProductToCart_WithInvalidQuantity_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Giving invalid quantity for product
            var productId = await this.SeedSingleProduct(context);
            var userId = await this.SeedSingleUser(context);
            var quantity = 0;

            var methodResult = await shoppingCartService.AddProductToCart(productId, userId, quantity);

            Assert.False(methodResult, "The method returned true with invalid quantity for product.");
        }

        [Fact]
        public async Task GetCartProductsByUserId_WithUserAndProducts_ShouldReturnCorrectProducts()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with products in his cart
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            string[] expectedProducts = new string[] { "firstName", "secondName", "thirdName" };
            int expectedCount = expectedProducts.Count();

            var methodResult = await shoppingCartService.GetCartProductsByUserId(userId);

            AssertExtensions.EqualCountWithMessage(methodResult.Count(), expectedCount, "The returned products count is not correct.");

            foreach (var product in methodResult)
            {
                Assert.True(expectedProducts.Contains(product.Name), "The returned products are not correct.");
            }
        }

        [Fact]
        public async Task GetCartProductsByUserId_WithNoUserProducts_ShouldReturnAnEmptyCollection()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding only User
            var userId = await this.SeedSingleUser(context);

            var methodResult = await shoppingCartService.GetCartProductsByUserId(userId);

            AssertExtensions.EmptyWithMessage(methodResult, "The method did not return an empty collection upon no products.");
        }

        [Fact]
        public async Task GetCartProductsByUserId_WithInvalidUser_ShouldReturnAnEmptyCollection()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding only User
            var userId = "FakeUserId";

            var methodResult = await shoppingCartService.GetCartProductsByUserId(userId);

            AssertExtensions.EmptyWithMessage(methodResult, "The method did not return an empty collection upon invalid user.");
        }

        [Fact]
        public async Task RemoveAllCartProducts_WithValidCartAndProducts_ShouldRemoveAllProductsSuccessfully()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products
            var userId = await this.SeedUserWithShoppingCartProducts(context);

            var methodResult = await shoppingCartService.RemoveAllCartProducts(userId);

            Assert.True(methodResult, "The method returned false with valid data.");

            var expectedCount = 0;
            var productsFromShoppingCart = context.ShoppingCarts
                .FirstOrDefault(sc => sc.ApplicationUserId == userId)
                .ShoppingCartProducts;

            AssertExtensions.EqualCountWithMessage(expectedCount, productsFromShoppingCart.Count, "The shopping cart was not empty after the method call.");
        }

        [Fact]
        public async Task RemoveAllCartProducts_WithInvalidUser_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding only User
            var userId = "FakeUserId";

            var methodResult = await shoppingCartService.RemoveAllCartProducts(userId);

            Assert.False(methodResult, "The method returned true with invalid user Id.");
        }

        [Fact]
        public async Task RemoveProductFromCart_WithValidUserAndProducts_ShouldRemoveProductSuccessfully()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            var productId = "firstProduct";

            var methodResult = await shoppingCartService.RemoveProductFromCart(productId, userId);

            Assert.True(methodResult, "The method returned false with valid data.");

            var expectedCount = 2;
            var productsFromShoppingCart = context.ShoppingCarts
                .FirstOrDefault(sc => sc.ApplicationUserId == userId)
                .ShoppingCartProducts;

            AssertExtensions.EqualCountWithMessage(expectedCount, productsFromShoppingCart.Count, "The product was not removed from the cart.");
        }

        [Fact]
        public async Task RemoveProductFromCart_WithInvalidUser_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding only Product
            var userId = "FakeUserId";
            var productId = await this.SeedSingleProduct(context);

            var methodResult = await shoppingCartService.RemoveProductFromCart(productId, userId);

            Assert.False(methodResult, "The method returned true with invalid user Id.");
        }

        [Fact]
        public async Task RemoveProductFromCart_WithInvalidProductId_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            var productId = "FakeProductId";

            var methodResult = await shoppingCartService.RemoveProductFromCart(productId, userId);

            Assert.False(methodResult, "The method returned true with invalid product Id.");
        }

        [Fact]
        public async Task UpdateProductCount_WithValidData_ShouldUpdateCountSuccessfully()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products
            // Will take the first product from all and change the quantity
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            var productId = "firstProduct";
            // The current quantity is 1
            var newQuantity = 4;

            var methodResult = await shoppingCartService.UpdateProductCount(productId, userId, newQuantity);

            Assert.True(methodResult, "The method returned false upon valid input.");

            var cartProductFromDatabase = await context.ShoppingCartProducts
                .FirstOrDefaultAsync(op => op.ProductId == productId);

            AssertExtensions.EqualCountWithMessage(newQuantity, cartProductFromDatabase.Quantity, "The product quantity was not changed.");
        }

        [Fact]
        public async Task UpdateProductCount_WithInvalidUser_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products but using fake userId
            var userId = "FakeUserId";
            await this.SeedUserWithShoppingCartProducts(context);
            var productId = "firstProduct";
            var newQuantity = 4;

            var methodResult = await shoppingCartService.UpdateProductCount(productId, userId, newQuantity);

            Assert.False(methodResult, "The method returned true upon invalid user Id.");
        }

        [Fact]
        public async Task UpdateProductCount_WithInvalidProduct_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products but using fake productId
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            var productId = "FakeProductId";
            var newQuantity = 4;

            var methodResult = await shoppingCartService.UpdateProductCount(productId, userId, newQuantity);

            Assert.False(methodResult, "The method returned true upon invalid product Id.");
        }

        [Fact]
        public async Task UpdateProductCount_WithInvalidQuantity_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var productService = new Mock<IProductService>();
            var shoppingCartService = new ShoppingCartService(context, productService.Object);

            // Seeding User with Products
            var userId = await this.SeedUserWithShoppingCartProducts(context);
            var productId = "FakeProductId";

            // Setting invalid quantity
            var newQuantity = 0;

            var methodResult = await shoppingCartService.UpdateProductCount(productId, userId, newQuantity);

            Assert.False(methodResult, "The method returned true upon invalid product quantity.");
        }

        // Session Cart tests not made
        public async Task<string> SeedUserWithShoppingCartProducts(ApplicationDbContext context)
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

        public async Task<string> SeedSingleUser(ApplicationDbContext context)
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

        public async Task<string> SeedSingleProduct(ApplicationDbContext context)
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

        public DbContextOptions<ApplicationDbContext> ConfigureContextOptionsAndAutoMapper()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(ErrorBindingModel).GetTypeInfo().Assembly);

            return options;
        }
    }
}
