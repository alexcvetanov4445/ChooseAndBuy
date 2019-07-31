namespace ChooseAndBuy.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.BindingModels;
    using ChooseAndBuy.Web.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class FavoritesServiceTests
    {
        [Fact]
        public async Task Add_WithCorrectData_ShouldProvideCorrectResult()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            // Gets the user(Usermanager accepts the id and returns the whole user) and productId only
            var user = this.GetUser();
            var productId = this.GetProduct().Id;

            var userManager = this.GetUserManager();
            userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seed both user and product
            await this.SeedUser(context);
            await this.SeedProduct(context);

            var methodResult = await favoritesService.Add(productId, user.Id);

            Assert.True(methodResult, "Service method returned false.");

            var favoriteProductFromDb = context
                .UsersFavoriteProducts
                .SingleOrDefaultAsync(x => x.ProductId == productId && x.ApplicationUserId == user.Id);

            AssertExtensions.NotNullWithMessage(favoriteProductFromDb, "The favorite product not found in database.");
        }

        [Fact]
        public async Task Add_WithNonExistingUser_MethodShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            // Gets the user(Usermanager accepts the fake id and returns null) and productId only
            ApplicationUser user = null;
            var userId = "fakeUserId";
            var productId = this.GetProduct().Id;

            var userManager = this.GetUserManager();
            userManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seed only product
            await this.SeedProduct(context);

            var methodResult = await favoritesService.Add(productId, userId);

            Assert.False(methodResult, "Method not returning false on non-existing user.");
        }

        [Fact]
        public async Task Add_WithNonExistingProduct_MethodShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            // Gets the user(Usermanager accepts the id and returns the whole user) and a fake productId
            var user = this.GetUser();
            var productId = "fakeProductId";

            var userManager = this.GetUserManager();
            userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seed only user
            await this.SeedUser(context);

            var methodResult = await favoritesService.Add(productId, user.Id);

            Assert.False(methodResult, "Method not returning false on non-existing product.");
        }

        [Fact]
        public async Task Remove_WithCorrectData_ShouldRemoveProductSuccessfully()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            // Gets the user(Usermanager accepts the id and returns the whole user) and a fake productId
            var user = this.GetUser();
            var productId = this.GetProduct().Id;

            var userManager = this.GetUserManager();
            userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seeding user, product and favorite product
            await this.SeedUser(context);
            await this.SeedProduct(context);
            await this.SeedFavoriteProduct(context);

            var methodResult = await favoritesService.Remove(productId, user.Id);

            Assert.True(methodResult, "The method does not return true upon successfull remove.");

            var doesExistInDatabase = context
                .UsersFavoriteProducts
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.ApplicationUserId == user.Id);

            Assert.True(doesExistInDatabase.Result == null, "The favorite product is not deleted from the database.");
        }

        [Fact]
        public async Task Remove_WithNonExistingUser_MethodShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            // Gets the user(Usermanager accepts the id and returns the whole user) and a fake productId
            ApplicationUser user = null;
            var userId = "fakeUserId";
            var productId = this.GetProduct().Id;

            var userManager = this.GetUserManager();
            userManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seeding user, product and favorite product (not matching userId)
            await this.SeedUser(context);
            await this.SeedProduct(context);
            await this.SeedFavoriteProduct(context);

            var methodResult = await favoritesService.Remove(productId, userId);

            Assert.False(methodResult, "The method does not return false upon non-existing user.");
        }

        [Fact]
        public async Task Remove_WithNonExistingProduct_MethodShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            // Gets the user(Usermanager accepts the id and returns the whole user) and a fake productId
            var user = this.GetUser();
            var productId = "FakeProductId";

            var userManager = this.GetUserManager();
            userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            // Seeding user, product and favorite product (not matching productId)
            await this.SeedUser(context);
            await this.SeedProduct(context);
            await this.SeedFavoriteProduct(context);

            var methodResult = await favoritesService.Remove(productId, user.Id);

            Assert.False(methodResult, "The method does not return false upon non-existing product.");
        }

        [Fact]
        public async Task GetUserFavorites_WithCorrectData_ShouldReturnAllFavoriteProducts()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            // Gets the user and products
            var user = this.GetUser();
            var products = this.GetMultipleProducts();

            // Seed the needed data
            await this.SeedUser(context);
            await this.SeedMultipleProducts(context);
            await this.SeedMultipleFavoriteProducts(context);

            var userManager = this.GetUserManager();
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

                Assert.True(products.Any(p => p.Id == productId && p.Name == productName), "The method did not return the correct products.");
            }

            AssertExtensions.EqualCountWithMessage(expectedCountOfProducts, actualCountOfProducts, "The count of the returned products is not correct");
        }

        [Fact]
        public async Task GetUserFavorites_WithNoFavoriteProducts_ShouldReturnEmptyCollection()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            // Gets the user
            var user = this.GetUser();

            // Seed the needed data
            await this.SeedUser(context);
            await this.SeedMultipleProducts(context);

            var userManager = this.GetUserManager();
            userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            var methodResult = await favoritesService.GetUserFavorites(user.Id);

            AssertExtensions.EmptyWithMessage(methodResult, "The method did not return an empty collection.");
        }

        [Fact]
        public async Task GetUserFavorites_WithNonExistingUser_ShouldReturnNull()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            // Sets a fake (non-existing user)
            ApplicationUser user = null;
            var userId = "fakeUserId";

            var userManager = this.GetUserManager();
            userManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            var favoritesService = new FavoritesService(context, userManager.Object);

            var methodResult = await favoritesService.GetUserFavorites(userId);

            AssertExtensions.NullWithMessage(methodResult, "The method did not return a null object.");
        }

        public async Task SeedUser(ApplicationDbContext context)
        {
            var user = this.GetUser();

            await context.Users.AddAsync(user);
        }

        public async Task SeedFavoriteProduct(ApplicationDbContext context)
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

        public async Task SeedMultipleFavoriteProducts(ApplicationDbContext context)
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

        public async Task SeedProduct(ApplicationDbContext context)
        {
            var product = this.GetProduct();

            await context.Products.AddAsync(product);

            await context.SaveChangesAsync();
        }

        public async Task SeedMultipleProducts(ApplicationDbContext context)
        {
            var products = this.GetMultipleProducts();

            await context.Products.AddRangeAsync(products);

            await context.SaveChangesAsync();
        }

        public Product GetProduct()
        {
            Product product = new Product
            {
                Id = "f9cf5158-31da-445c-9667-484a2f01dbf7",
                Name = "TestProduct",
                Price = 10,
            };

            return product;
        }

        public ICollection<Product> GetMultipleProducts()
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

        public ApplicationUser GetUser()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "827c74c1-46ab-489d-914e-431297f55a7b",
                UserName = "TestUsername",
            };

            return user;
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

        public Mock<UserManager<ApplicationUser>> GetUserManager()
        {
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            return mockUserManager;
        }
    }
}
