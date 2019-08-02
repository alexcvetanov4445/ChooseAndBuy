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
    using ChooseAndBuy.Web.BindingModels.Products;
    using ChooseAndBuy.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ReviewServiceTests
    {
        [Fact]
        public async Task AddReview_WithCorrectData_ShouldCreateSuccessfully()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var reviewService = new ReviewService(context);

            ReviewBindingModel model = new ReviewBindingModel
            {
                ClientFullName = "TestClient",
                Comment = "TestComment",
                Rating = 5,
            };

            var methodResult = await reviewService.AddReview(model);

            Assert.True(methodResult, "The method returned false on valid creation model.");

            var reviewExists = context.Reviews.FirstOrDefaultAsync(x => x.ClientFullName == model.ClientFullName);

            AssertExtensions.NotNullWithMessage(reviewExists, "The review was not added to the database.");
        }

        [Fact]
        public async Task GetReviewsForProduct_WithSeededReviews_ShouldReturnCorrectReviews()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var reviewService = new ReviewService(context);

            // Seeding 3 reviews for 1 product with Id - TestProductId
            var expectedReviews = await this.SeedAndGetReviewsForProduct(context);

            var productId = "TestProductId";

            var methodResult = await reviewService.GetReviewsForProduct(productId);

            Assert.True(methodResult.Any(x => expectedReviews.Any(y => y.ClientFullName == x.ClientFullName)), "The returned reviews are not correct.");
        }

        [Fact]
        public async Task GetReviewsForProduct_WithNoExistingProduct_ShouldReturnEmptyCollection()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var reviewService = new ReviewService(context);

            var productId = "TestProductId";

            var methodResult = await reviewService.GetReviewsForProduct(productId);

            AssertExtensions.EmptyWithMessage(methodResult, "The method did not return an empty collection.");
        }

        public async Task<List<Review>> SeedAndGetReviewsForProduct(ApplicationDbContext context)
        {
            var product = new Product
            {
                Id = "TestProductId",
                Name = "TestProductName",
            };

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            List<Review> reviews = new List<Review>()
            {
                new Review
                {
                    Id = "First",
                    ProductId = "TestProductId",
                    ClientFullName = "FirstClientName",
                },
                new Review
                {
                    Id = "Second",
                    ProductId = "TestProductId",
                    ClientFullName = "SecondClientName",
                },
                new Review
                {
                    Id = "Third",
                    ProductId = "TestProductId",
                    ClientFullName = "ThirdClientName",
                },
            };

            await context.Reviews.AddRangeAsync(reviews);
            await context.SaveChangesAsync();

            return reviews;
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
