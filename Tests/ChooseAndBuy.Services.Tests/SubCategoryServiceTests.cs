namespace ChooseAndBuy.Services.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Tests.Common;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.BindingModels.Administration.SubCategories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class SubCategoryServiceTests
    {
        private async Task<string> SeedCategory(ApplicationDbContext context)
        {
            var category = new Category
            {
                Id = "FirstCategoryId",
                Name = "FirstCategoryName",
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return category.Id;
        }

        private async Task<ICollection<SubCategory>> SeedAndGetSubCategoriesWithCategories(ApplicationDbContext context)
        {
            // Set the main category for the sub-categories
            var firstCategory = new Category
            {
                Id = "FirstCategoryId",
                Name = "FirstCategoryName",
            };

            var secondCategory = new Category
            {
                Id = "SecondPrimaryId",
                Name = "SecondCategoryName",
            };

            await context.Categories.AddAsync(firstCategory);
            await context.Categories.AddAsync(secondCategory);
            await context.SaveChangesAsync();

            // Set the sub-categories with the main category Id

            var subCategories = new List<SubCategory>()
            {
                // For the first category - 3 sub-categories
                new SubCategory
                {
                    CategoryId = firstCategory.Id,
                    Id = "A-SubCategoryId-forFirst",
                    Name = "A-SubCategory-forFirst",
                },
                new SubCategory
                {
                    CategoryId = firstCategory.Id,
                    Id = "B-SubCategoryId-forFirst",
                    Name = "B-SubCategory-forFirst",
                },
                new SubCategory
                {
                    CategoryId = firstCategory.Id,
                    Id = "C-SubCategoryId-forFirst",
                    Name = "C-SubCategory-forFirst",
                },

                // For the second category - 3 sub-categories
                new SubCategory
                {
                    CategoryId = secondCategory.Id,
                    Id = "A-SubCategoryId-forSecond",
                    Name = "A-SubCategory-forSecond",
                },
                new SubCategory
                {
                    CategoryId = secondCategory.Id,
                    Id = "B-SubCategoryId-forSecond",
                    Name = "B-SubCategory-forSecond",
                },
                new SubCategory
                {
                    CategoryId = secondCategory.Id,
                    Id = "C-SubCategoryId-forSecond",
                    Name = "C-SubCategory-forSecond",
                },
            };

            await context.AddRangeAsync(subCategories);
            await context.SaveChangesAsync();

            var subCategoriesWithCategories = await context.SubCategories.Include(c => c.Category).ToListAsync();

            return subCategoriesWithCategories;
        }

        public SubCategoryServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task AddSubCategory_WithCorrectData_ShouldCreateSubCategorySuccessfully()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var subCategoryService = new SubCategoryService(context);

            // Seed only one main category
            var categoryId = await this.SeedCategory(context);

            SubCategoryBindingModel model = new SubCategoryBindingModel
            {
                CategoryId = categoryId,
                Name = "NewSubCategory",
            };

            var methodResult = await subCategoryService.AddSubCategory(model);

            Assert.True(methodResult, "The method returned false on correct input for creation.");

            var subCategoryFromDatabase = await context.SubCategories.FirstOrDefaultAsync(c => c.Name == model.Name);

            AssertExtensions.NotNullWithMessage(subCategoryFromDatabase, "The subCategory was not found in the database.");
        }

        [Fact]
        public async Task AddSubCategory_WithNonExistingMainCategory_ShouldReturnFalse()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var subCategoryService = new SubCategoryService(context);

            string categoryId = "fakeCategoryId";

            SubCategoryBindingModel model = new SubCategoryBindingModel
            {
                CategoryId = categoryId,
                Name = "NewSubCategory",
            };

            var methodResult = await subCategoryService.AddSubCategory(model);

            Assert.False(methodResult, "The method did not return false upon wrong data input for creation.");
        }

        [Fact]
        public async Task GetSubCategories_WithSeededSubCategories_ShouldReturnCorrectResult()
        {
            string onFalseErrorMessage = "The returned sub-categories are not correct.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var subCategoryService = new SubCategoryService(context);

            // Seeding multiple categories with sub-categories
            var expectedSubCategories = await this.SeedAndGetSubCategoriesWithCategories(context);

            var actualSubCategories = await subCategoryService.GetSubCategories();

            // The returned names contains the following format -> "SubCategoryName(MainCategoryName)"
            foreach (var ctg in actualSubCategories)
            {
                Assert.True(
                    expectedSubCategories.Any(c => c.Name + $"({c.Category.Name})" == ctg.Text && c.Id == ctg.Value),
                    onFalseErrorMessage);
            }
        }

        [Fact]
        public async Task GetSubCategories_WithNoSubCategories_ShouldReturnAnEmptyCollection()
        {
            string onCountDifferenceErrorMessage = "The method did not return an empty collection.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var subCategoryService = new SubCategoryService(context);

            var methodResult = await subCategoryService.GetSubCategories();

            var expectedCount = 0;

            AssertExtensions.EqualCountWithMessage(
                expectedCount, 
                methodResult.Count(), 
                onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task SubCategoryExists_OnExistingSubCategory_ShouldReturnTrue()
        {
            string onFalseErrorMessage = "The method returned false on existing sub-category.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var subCategoryService = new SubCategoryService(context);

            // Seeding multiple categories with sub-categories
            var expectedSubCategories = await this.SeedAndGetSubCategoriesWithCategories(context);

            string existingSubCategoryName = "A-SubCategory-forSecond";

            var methodResult = await subCategoryService.SubCategoryExists(existingSubCategoryName);

            Assert.True(methodResult, onFalseErrorMessage);
        }

        [Fact]
        public async Task SubCategoryExists_OnNonExistingSubCategory_ShouldReturnTrue()
        {
            string onTrueErrorMessage = "The method returned true on non-existing sub-category.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var subCategoryService = new SubCategoryService(context);

            string nonExistingSubCategoryName = "fakeSubCategoryId";

            var methodResult = await subCategoryService.SubCategoryExists(nonExistingSubCategoryName);

            Assert.False(methodResult, onTrueErrorMessage);
        }
    }
}
