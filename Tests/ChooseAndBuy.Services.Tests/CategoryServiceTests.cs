namespace ChooseAndBuy.Services.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Tests.Common;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.BindingModels.Administration.Categories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CategoryServiceTests
    {
        private ICollection<SubCategory> GetSubCategories()
        {
            var subCategories = new List<SubCategory>
            {
                new SubCategory
                {
                    Name = "Test-First-sc",
                    CategoryId = "First-Id",
                },
                new SubCategory
                {
                    Name = "Test-Second-sc",
                    CategoryId = "Second-Id",
                },
                new SubCategory
                {
                    Name = "Test-Third-sc",
                    CategoryId = "Third-Id",
                },
            };

            return subCategories;
        }

        private ICollection<Category> GetCategories()
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Id = "First-Id",
                    Name = "Test-First",
                },
                new Category
                {
                    Id = "Second-Id",
                    Name = "Test-Second",
                },
                new Category
                {
                    Id = "Third-Id",
                    Name = "Test-Third",
                },
            };

            return categories;
        }

        private async Task SeedCategories(ApplicationDbContext context)
        {
            var categories = this.GetCategories();

            await context.Categories.AddRangeAsync(categories);

            await context.SaveChangesAsync();
        }

        private async Task SeedCategoriesWithSubCategories(ApplicationDbContext context)
        {
            var categories = this.GetCategories();
            var subcategories = this.GetSubCategories();

            await context.AddRangeAsync(categories);
            await context.AddRangeAsync(subcategories);

            await context.SaveChangesAsync();
        }

        public CategoryServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task AddCategory_ShouldCreateCategorySuccessfully()
        {
            string onFalseErrorMessage = "Method returned false bool.";
            string onNullErrorMessage = "Category was not stored in the database.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryService = new CategoryService(context);

            CreateCategoryBindingModel model =
                new CreateCategoryBindingModel
                {
                    Name = "TestCategory",
                };

            var methodResult = await categoryService.AddCategory(model);
            Assert.True(methodResult, onFalseErrorMessage);

            var categoryFromDb = context.Categories.FirstOrDefaultAsync();
            AssertExtensions.NotNullWithMessage(categoryFromDb, onNullErrorMessage);
        }

        [Fact]
        public async Task GetCategories_WithData_ShouldReturnCorrectData()
        {
            string onFalseErrorMessage = "Returned categories are not correct.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryService = new CategoryService(context);

            await this.SeedCategories(context);

            var expectedCategories = this.GetCategories();
            var actualCategories = await categoryService.GetCategories();

            foreach (var actualCategory in actualCategories)
            {
                Assert.True(expectedCategories.Any(x => x.Name == actualCategory.Text), onFalseErrorMessage);
            }
        }

        [Fact]
        public async Task GetCategories_WithoutData_ShouldReturnEmptyCollection()
        {
            string onNonEmptyCollectionErrorMessage = "The method does not return an empty collection.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryService = new CategoryService(context);

            var actualCategories = await categoryService.GetCategories();

            AssertExtensions.EmptyWithMessage(actualCategories, onNonEmptyCollectionErrorMessage);
        }

        [Fact]
        public async Task GetCategoriesWithSubCategories_WithData_ShouldReturnCorrectData()
        {
            string onFalseWithCategoryErrorMessage = "The Category names don't match.";
            string onFalseWithSubCategoryErrorMessage = "The SubCategory name don't match.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryService = new CategoryService(context);

            await this.SeedCategoriesWithSubCategories(context);

            var expectedCategories = this.GetCategories();
            var expectedSubCategories = this.GetSubCategories();

            var actualCategoriesWithSubCategories = await categoryService.GetCategoriesWithSubCategories();

            foreach (var ctg in actualCategoriesWithSubCategories)
            {
                // ctg -> string(category) with SelectListItem Collection(subcategory)
                var categoryName = ctg.Key;
                var subCategories = ctg.Value;

                Assert.True(
                    expectedCategories.Any(x => x.Name == categoryName),
                    onFalseWithCategoryErrorMessage);

                Assert.True(
                    expectedSubCategories.Any(x => subCategories.Any(y => y.Text == x.Name)),
                    onFalseWithSubCategoryErrorMessage);
            }
        }

        [Fact]
        public async Task GetCategoriesWithSubCategories_WithoutData_ShouldReturnEmptyCollection()
        {
            string onNonEmptyCollectionErrorMessage = "The returned collection is not empty or it's null";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryService = new CategoryService(context);

            var actualCategoriesWithSubCategories = await categoryService.GetCategoriesWithSubCategories();

            AssertExtensions.EmptyWithMessage(
                actualCategoriesWithSubCategories,
                onNonEmptyCollectionErrorMessage);
        }

        [Fact]
        public async Task ValidateCategoryName_WithExistingCategoryName_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true on existing category name.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedCategories(context);

            var categoryService = new CategoryService(context);

            var firstCategory = this.GetCategories().First();

            var result = await categoryService.ValidateCategoryName(firstCategory.Name);

            Assert.False(result, onTrueErrorMessage);
        }

        [Fact]
        public async Task ValidateCategoryName_WithNonExistingCategoryName_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned false on non-existing category name.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryService = new CategoryService(context);

            var firstCategory = this.GetCategories().First();

            var result = await categoryService.ValidateCategoryName(firstCategory.Name);

            Assert.True(result, onTrueErrorMessage);
        }

        [Fact]
        public async Task DeleteCategory_WithExistingCategory_ShouldDeleteCategorySuccessfully()
        {
            string onFalseErrorMessage = "The method returned false upon valid category input.";
            string onNotNullErrorMessage = "The category is not deleted from the database.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryService = new CategoryService(context);

            await this.SeedCategories(context);

            string validCategoryId = "First-Id";

            var methodResult = await categoryService.DeleteCategory(validCategoryId);

            Assert.True(methodResult, onFalseErrorMessage);

            var categoriesFromDb = await context
                .Categories
                .FirstOrDefaultAsync(c => c.Id == validCategoryId);

            AssertExtensions.NullWithMessage(categoriesFromDb, onNotNullErrorMessage);
        }

        [Fact]
        public async Task DeleteCategory_WithNonExistingCategory_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true upon valid category input.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryService = new CategoryService(context);

            string invalidCategoryId = "FakeCategoryId";

            var methodResult = await categoryService.DeleteCategory(invalidCategoryId);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task GetDeletableCategories_WithDeletableSubCategories_ShouldReturnCorrectCollection()
        {
            string onCountDiferenceErrorMessage = "The method did not return the expected collection with elements.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryService = new CategoryService(context);

            await this.SeedCategories(context);

            var methodResult = await categoryService.GetDeletableCategories();

            var expectedCount = 3;

            AssertExtensions.EqualCountWithMessage(expectedCount, methodResult.Count(), onCountDiferenceErrorMessage);
        }

        [Fact]
        public async Task GetDeletableCategories_WithNonDeletableSubCategories_ShouldReturnCorrectCollection()
        {
            string onCountDiferenceErrorMessage = "The method did not return the expected collection with elements.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryService = new CategoryService(context);

            // seeding categories with sub-categories attached to them so they cannot be deleted
            await this.SeedCategoriesWithSubCategories(context);

            var methodResult = await categoryService.GetDeletableCategories();

            var expectedCount = 0;

            AssertExtensions.EqualCountWithMessage(expectedCount, methodResult.Count(), onCountDiferenceErrorMessage);
        }

        [Fact]
        public async Task GetDeletableCategories_WithNoCategories_ShouldReturnAnEmptyCollection()
        {
            string onCountDiferenceErrorMessage = "The method did not return an empty collection.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryService = new CategoryService(context);

            var methodResult = await categoryService.GetDeletableCategories();

            var expectedCount = 0;

            AssertExtensions.EqualCountWithMessage(expectedCount, methodResult.Count(), onCountDiferenceErrorMessage);
        }
    }
}
