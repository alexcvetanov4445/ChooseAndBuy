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
    using ChooseAndBuy.Web.BindingModels.Administration.Categories;
    using ChooseAndBuy.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CategoryServiceTests
    {
        [Fact]
        public async Task AddCategory_ShouldCreateCategorySuccessfully()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var categoryService = new CategoryService(context);

            CreateCategoryBindingModel model =
                new CreateCategoryBindingModel
                {
                    Name = "TestCategory",
                };

            var methodResult = await categoryService.AddCategory(model);
            Assert.True(methodResult, "Method returned false bool.");

            var categoryFromDb = context.Categories.FirstOrDefaultAsync();
            AssertExtensions.NotNullWithMessage(categoryFromDb, "Category was not stored in the database.");
        }

        [Fact]
        public async Task GetCategories_WithData_ShouldReturnCorrectData()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var categoryService = new CategoryService(context);

            await this.SeedCategories(context);

            var expectedCategories = this.GetCategories();
            var actualCategories = await categoryService.GetCategories();

            foreach (var actualCategory in actualCategories)
            {
                Assert.True(expectedCategories.Any(x => x.Name == actualCategory.Text), "Returned categories are not correct.");
            }
        }

        [Fact]
        public async Task GetCategories_WithoutData_ShouldReturnEmptyCollection()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var categoryService = new CategoryService(context);

            var actualCategories = await categoryService.GetCategories();

            AssertExtensions.EmptyWithMessage(actualCategories, "The method does not return an empty collection.");
        }

        [Fact]
        public async Task GetCategoriesWithSubCategories_WithData_ShouldReturnCorrectData()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

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
                    "The Category names don't match.");

                Assert.True(
                    expectedSubCategories.Any(x => subCategories.Any(y => y.Text == x.Name)),
                    "The SubCategory name don't match.");
            }
        }

        [Fact]
        public async Task GetCategoriesWithSubCategories_WithoutData_ShouldReturnEmptyCollection()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var categoryService = new CategoryService(context);

            var actualCategoriesWithSubCategories = await categoryService.GetCategoriesWithSubCategories();

            AssertExtensions.EmptyWithMessage(
                actualCategoriesWithSubCategories,
                "The returned collection is not empty or it's null");
        }

        [Fact]
        public async Task ValidateCategoryName_WithExistingCategoryName_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            await this.SeedCategories(context);

            var categoryService = new CategoryService(context);

            var firstCategory = this.GetCategories().First();

            var result = await categoryService.ValidateCategoryName(firstCategory.Name);

            Assert.False(result, "The method returned true on existing category name.");
        }

        [Fact]
        public async Task ValidateCategoryName_WithNonExistingCategoryName_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var categoryService = new CategoryService(context);

            var firstCategory = this.GetCategories().First();

            var result = await categoryService.ValidateCategoryName(firstCategory.Name);

            Assert.True(result, "The method returned false on non-existing category name.");
        }

        public async Task SeedCategories(ApplicationDbContext context)
        {
            var categories = this.GetCategories();

            await context.Categories.AddRangeAsync(categories);

            await context.SaveChangesAsync();
        }

        public async Task SeedCategoriesWithSubCategories(ApplicationDbContext context)
        {
            var categories = this.GetCategories();
            var subcategories = this.GetSubCategories();

            await context.AddRangeAsync(categories);
            await context.AddRangeAsync(subcategories);

            await context.SaveChangesAsync();
        }

        public ICollection<SubCategory> GetSubCategories()
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

        public ICollection<Category> GetCategories()
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
