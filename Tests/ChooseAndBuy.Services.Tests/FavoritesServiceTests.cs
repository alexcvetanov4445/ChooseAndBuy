namespace ChooseAndBuy.Services.Tests
{
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

    public class FavoritesServiceTests
    {
        private ApplicationUser GetUser()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "827c74c1-46ab-489d-914e-431297f55a7b",
                UserName = "TestUsername",
            };

            return user;
        }

        private Product GetProduct()
        {
            Product product = new Product
            {
                Id = "f9cf5158-31da-445c-9667-484a2f01dbf7",
                Name = "TestProduct",
                Price = 10,
            };

            return product;
        }

        private ICollection<Product> GetMultipleProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = "TestProductId1",
                    Name = "TestProduct1",
                },
                new Product
                {
                    Id = "TestProductId2",
                    Name = "TestProduct2",
                },
                new Product
                {
                    Id = "TestProductId3",
                    Name = "TestProduct3",
                },
            };

            return products;
        }

        private async Task SeedUser(ApplicationDbContext context)
        {
            var user = this.GetUser();

            await context.Users.AddAsync(user);
        }

        private async Task SeedProduct(ApplicationDbContext context)
        {
            var product = this.GetProduct();

            await context.Products.AddAsync(product);

            await context.SaveChangesAsync();
        }

        private async Task SeedFavoriteProduct(ApplicationDbContext context)
        {
            var productId = this.GetProduct().Id;
            var userId = this.GetUser().Id;

            UserFavoriteProduct favoriteProduct = new UserFavoriteProduct
            {
                ApplicationUserId = userId,
                ProductId = productId,
            };

            await context.UsersFavoriteProducts.AddAsync(favoriteProduct);

            await context.SaveChangesAsync();
        }

        private async Task SeedMultipleProducts(ApplicationDbContext context)
        {
            var products = this.GetMultipleProducts();

            await context.Products.AddRangeAsync(products);

            await context.SaveChangesAsync();
        }

        private async Task SeedMultipleFavoriteProducts(ApplicationDbContext context)
        {
            var userId = this.GetUser().Id;

            var favProducts = new List<UserFavoriteProduct>()
            {
                new UserFavoriteProduct
                {
                    ApplicationUserId = userId,
                    ProductId = "TestProductId1",
                },
                new UserFavoriteProduct
                {
                    ApplicationUserId = userId,
                    ProductId = "TestProductId2",
                },
                new UserFavoriteProduct
                {
                    ApplicationUserId = userId,
                    ProductId = "TestProductId3",
                },
            };

            await context.UsersFavoriteProducts.AddRangeAsync(favProducts);

            await context.SaveChangesAsync();
        }

        public FavoritesServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task Add_WithCorrectData_ShouldProvideCorrectResult()
        {
            string onFalseErrorMessage = "Service method returned false.";
            string onNullErrorMessage = "The favorite product not found in database.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            // Gets the user(Usermanager accepts the id and returns the whole user) and productId only
            var user = this.GetUser();
            var productId = this.GetProduct().Id;

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seed both user and product
            await this.SeedUser(context);
            await this.SeedProduct(context);

            var methodResult = await favoritesService.Add(productId, user.Id);

            Assert.True(methodResult, onFalseErrorMessage);

            var favoriteProductFromDb = context
                .UsersFavoriteProducts
                .SingleOrDefaultAsync(x => x.ProductId == productId && x.ApplicationUserId == user.Id);

            AssertExtensions.NotNullWithMessage(favoriteProductFromDb, onNullErrorMessage);
        }

        [Fact]
        public async Task Add_WithNonExistingUser_MethodShouldReturnFalse()
        {
            string onTrueErrorMessage = "Method not returning false on non-existing user.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            // Gets the user(Usermanager accepts the fake id and returns null) and productId only
            ApplicationUser user = null;
            var userId = "fakeUserId";
            var productId = this.GetProduct().Id;

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            userManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seed only product
            await this.SeedProduct(context);

            var methodResult = await favoritesService.Add(productId, userId);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task Add_WithNonExistingProduct_MethodShouldReturnFalse()
        {
            string onTrueErrorMessage = "Method not returning false on non-existing product.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            // Gets the user(Usermanager accepts the id and returns the whole user) and a fake productId
            var user = this.GetUser();
            var productId = "fakeProductId";

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seed only user
            await this.SeedUser(context);

            var methodResult = await favoritesService.Add(productId, user.Id);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task Remove_WithCorrectData_ShouldRemoveProductSuccessfully()
        {
            string onFalseMethodReturnErrorMessage = "The method does not return true upon successfull remove.";
            string onFalseDatabaseReturnErrorMessage = "The favorite product is not deleted from the database.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            // Gets the user(Usermanager accepts the id and returns the whole user) and a fake productId
            var user = this.GetUser();
            var productId = this.GetProduct().Id;

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seeding user, product and favorite product
            await this.SeedUser(context);
            await this.SeedProduct(context);
            await this.SeedFavoriteProduct(context);

            var methodResult = await favoritesService.Remove(productId, user.Id);

            Assert.True(methodResult, onFalseMethodReturnErrorMessage);

            var doesExistInDatabase = context
                .UsersFavoriteProducts
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.ApplicationUserId == user.Id);

            Assert.True(doesExistInDatabase.Result == null, onFalseDatabaseReturnErrorMessage);
        }

        [Fact]
        public async Task Remove_WithNonExistingUser_MethodShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method does not return false upon non-existing user.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            // Gets the user(Usermanager accepts the id and returns the whole user) and a fake productId
            ApplicationUser user = null;
            var userId = "fakeUserId";
            var productId = this.GetProduct().Id;

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            userManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seeding user, product and favorite product (not matching userId)
            await this.SeedUser(context);
            await this.SeedProduct(context);
            await this.SeedFavoriteProduct(context);

            var methodResult = await favoritesService.Remove(productId, userId);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task Remove_WithNonExistingProduct_MethodShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method does not return false upon non-existing product.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            // Gets the user(Usermanager accepts the id and returns the whole user) and a fake productId
            var user = this.GetUser();
            var productId = "FakeProductId";

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seeding user, product and favorite product (not matching productId)
            await this.SeedUser(context);
            await this.SeedProduct(context);
            await this.SeedFavoriteProduct(context);

            var methodResult = await favoritesService.Remove(productId, user.Id);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task GetUserFavorites_WithCorrectData_ShouldReturnAllFavoriteProducts()
        {
            string onFalseErrorMessage = "The method did not return the correct products.";
            string onCountDifferenceErrorMessage = "The count of the returned products is not correct";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            // Gets the user and products
            var user = this.GetUser();
            var products = this.GetMultipleProducts();

            // Seed the needed data
            await this.SeedUser(context);
            await this.SeedMultipleProducts(context);
            await this.SeedMultipleFavoriteProducts(context);

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            var methodResult = await favoritesService.GetUserFavorites(user.Id);

            var expectedCountOfProducts = products.Count;
            var actualCountOfProducts = 0;

            foreach (var pr in methodResult)
            {
                actualCountOfProducts++;
                var productId = pr.Id;
                var productName = pr.Name;

                Assert.True(
                    products.Any(p => p.Id == productId && p.Name == productName), 
                    onFalseErrorMessage);
            }

            AssertExtensions.EqualCountWithMessage(
                expectedCountOfProducts, 
                actualCountOfProducts, 
                onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task GetUserFavorites_WithNoFavoriteProducts_ShouldReturnEmptyCollection()
        {
            string onNonEmptyCollectionErrorMessage = "The method did not return an empty collection.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            // Gets the user
            var user = this.GetUser();

            // Seed the needed data
            await this.SeedUser(context);
            await this.SeedMultipleProducts(context);

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            var methodResult = await favoritesService.GetUserFavorites(user.Id);

            AssertExtensions.EmptyWithMessage(methodResult, onNonEmptyCollectionErrorMessage);
        }

        [Fact]
        public async Task GetUserFavorites_WithNonExistingUser_ShouldReturnNull()
        {
            string onNonNullErrorMessage = "The method did not return a null object.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            // Sets a fake (non-existing user)
            ApplicationUser user = null;
            var userId = "fakeUserId";

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            userManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            var methodResult = await favoritesService.GetUserFavorites(userId);

            AssertExtensions.NullWithMessage(methodResult, onNonNullErrorMessage);
        }
    }
}
