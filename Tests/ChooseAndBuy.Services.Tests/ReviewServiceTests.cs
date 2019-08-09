namespace ChooseAndBuy.Services.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Tests.Common;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.BindingModels.Products;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ReviewServiceTests
    {
        private async Task<List<Review>> SeedAndGetReviewsForProduct(ApplicationDbContext context)
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

        public ReviewServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task AddReview_WithCorrectData_ShouldCreateSuccessfully()
        {
            string onFalseErrorMessage = "The method returned false on valid creation model.";
            string onNullErrorMessage = "The review was not added to the database.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var reviewService = new ReviewService(context);

            ReviewBindingModel model = new ReviewBindingModel
            {
                ClientFullName = "TestClient",
                Comment = "TestComment",
                Rating = 5,
            };

            var methodResult = await reviewService.AddReview(model);

            Assert.True(methodResult, onFalseErrorMessage);

            var reviewExists = context.Reviews.FirstOrDefaultAsync(x => x.ClientFullName == model.ClientFullName);

            AssertExtensions.NotNullWithMessage(reviewExists, onNullErrorMessage);
        }

        [Fact]
        public async Task GetReviewsForProduct_WithSeededReviews_ShouldReturnCorrectReviews()
        {
            string onFalseErrorMessage = "The returned reviews are not correct.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var reviewService = new ReviewService(context);

            // Seeding 3 reviews for 1 product with Id - TestProductId
            var expectedReviews = await this.SeedAndGetReviewsForProduct(context);

            var productId = "TestProductId";

            var methodResult = await reviewService.GetReviewsForProduct(productId);

            Assert.True(
                methodResult.Any(x => expectedReviews.Any(y => y.ClientFullName == x.ClientFullName)),
                onFalseErrorMessage);
        }

        [Fact]
        public async Task GetReviewsForProduct_WithNoExistingProduct_ShouldReturnEmptyCollection()
        {
            string onNonEmptyCollectionErrorMessage = "The method did not return an empty collection.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var reviewService = new ReviewService(context);

            var productId = "TestProductId";

            var methodResult = await reviewService.GetReviewsForProduct(productId);

            AssertExtensions.EmptyWithMessage(methodResult, onNonEmptyCollectionErrorMessage);
        }


    }
}
