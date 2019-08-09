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
    using ChooseAndBuy.Services.Tests.Common;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.BindingModels;
    using ChooseAndBuy.Web.BindingModels.Administration.Products;
    using ChooseAndBuy.Web.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ProductServiceTests
    {
        private Product GetProduct()
        {
            Product product = new Product
            {
                Id = "TestId",
                Price = 10,
                Name = "TestProduct",
                Description = "TestDesc",
                Specification = "TestSpec",
            };

            return product;
        }

        private ICollection<Product> GetMultipleProducts()
        {
            var products = new List<Product>
            {
                // Set 8 products for each sub-category
                // 5 products are for the first main Category
                // 3 products are for the second main Category
                // 3 with duplicating sub-category as the first(for validating purposes)
                // 2 products are recommended
                new Product
                {
                    Id = "A-firstProductId",
                    Name = "A-firstProductName",

                    // duplicate
                    SubCategoryId = "A-SubCategoryId-forFirst",
                },
                new Product
                {
                    Id = "B-firstProductId",
                    Name = "B-firstProductName",

                    // duplicate
                    SubCategoryId = "A-SubCategoryId-forFirst",
                },
                new Product
                {
                    Id = "B-ProductId",
                    Name = "B-ProductName",

                    // duplicate
                    SubCategoryId = "A-SubCategoryId-forFirst",
                },
                new Product
                {
                    Id = "secondProductId",
                    Name = "secondProductName",
                    SubCategoryId = "B-SubCategoryId-forFirst",
                },
                new Product
                {
                    Id = "thirdProductId",
                    Name = "thirdProductName",
                    SubCategoryId = "C-SubCategoryId-forFirst",
                    IsRecommended = true,
                },
                new Product
                {
                    Id = "forthProductId",
                    Name = "forthProductName",
                    SubCategoryId = "A-SubCategoryId-forSecond",
                },
                new Product
                {
                    Id = "fifthProductId",
                    Name = "fifthProductName",
                    SubCategoryId = "B-SubCategoryId-forSecond",
                },
                new Product
                {
                    Id = "sixthProductId",
                    Name = "sixthProductName",
                    SubCategoryId = "C-SubCategoryId-forSecond",
                    IsRecommended = true,
                },
            };

            return products;
        }

        private async Task SeedMultipleProducts(ApplicationDbContext context)
        {
            var products = this.GetMultipleProducts();

            await context.Products.AddRangeAsync(products);

            await context.SaveChangesAsync();
        }

        private async Task SeedMultipleSubCategoriesAndCategories(ApplicationDbContext context)
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
        }

        private async Task SeedSingleProduct(ApplicationDbContext context)
        {
            var product = this.GetProduct();

            await context.Products.AddAsync(product);

            await context.SaveChangesAsync();
        }

        public ProductServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task ProductExists_WithNonExistingProduct_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true on non-existing product.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get the test product and do not seed it
            var product = this.GetProduct();

            var methodResult = await productService.ProductExists(product.Name);

            AssertExtensions.FalseWithMessage(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task ProductExists_WithExistingProduct_ShouldReturnTrue()
        {
            string onTrueErrorMessage = "The method returned false on existing product.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get the test product and seed it
            var product = this.GetProduct();
            await this.SeedSingleProduct(context);

            var methodResult = await productService.ProductExists(product.Name);

            Assert.True(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task AddProduct_WithCorrectData_ShouldAddproductSuccessfully()
        {
            string onFalseErrorMessage = "The method returned false upon correct product data.";
            string onNullErrorMessage = "There was no product returned from the database.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            // Creating the neccessary dependencies for mocking the cloudinary service with method "CreateImage"
            IFormFile file = null;
            var imageName = "mockCreateImage";
            var cloudinaryServiceMock = new Mock<ICloudinaryService>();
            cloudinaryServiceMock.Setup(m => m.CreateImage(file, "file")).ReturnsAsync(imageName);

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            CreateProductBindingModel model = new CreateProductBindingModel
            {
                Name = "TestProduct",
                Price = 10,
            };

            var methodResult = await productService.AddProduct(model);

            Assert.True(methodResult, onFalseErrorMessage);

            // Checking the product in database
            var dbProduct = await context.Products.FirstOrDefaultAsync(p => p.Name == model.Name);

            AssertExtensions.NotNullWithMessage(dbProduct, onNullErrorMessage);
        }

        [Fact]
        public async Task HideProduct_WithExistingProduct_ShouldReturnTrue()
        {
            string onFalseErrorMessage = "The method returned false on existing product.";
            string onNullErrorMessage = "The product was either not in the database or not hidden.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get the product and seed it
            var product = this.GetProduct();
            await this.SeedSingleProduct(context);

            var methodResult = await productService.HideProduct(product.Id);

            Assert.True(methodResult, onFalseErrorMessage);

            var productFromDatabase = context.Products.FirstOrDefaultAsync(p => p.IsHidden == true);

            AssertExtensions.NotNullWithMessage(productFromDatabase, onNullErrorMessage);
        }

        [Fact]
        public async Task HideProduct_WithNonExistingProduct_ShouldReturnTrue()
        {
            string onTrueErrorMessage = "The method returned true on non-existing product.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get the product and do not seed it
            var product = this.GetProduct();

            var methodResult = await productService.HideProduct(product.Id);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task RecommendProduct_WithExistingProduct_ShouldReturnTrue()
        {
            string onFalseErrorMessage = "The method returned false on existing product.";
            string onNullErrorMessage = "The product was either not in the database or not recommended.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get the product and seed it
            var product = this.GetProduct();
            await this.SeedSingleProduct(context);

            var methodResult = await productService.RecommendProduct(product.Id);

            Assert.True(methodResult, onFalseErrorMessage);

            var productFromDatabase = context.Products.FirstOrDefaultAsync(p => p.IsHidden == true);

            AssertExtensions.NotNullWithMessage(productFromDatabase, onNullErrorMessage);
        }

        [Fact]
        public async Task RecommendProduct_WithNonExistingProduct_ShouldReturnTrue()
        {
            string onTrueErrorMessage = "The method returned true on non-existing product.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get the product and do not seed it
            var product = this.GetProduct();

            var methodResult = await productService.RecommendProduct(product.Id);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task EditProduct_WithCorrectData_ShouldEditProductSuccessfully()
        {
            string onFalseErrorMessage = "The method returned false upon correct data for edit.";
            string onNullErrorMessage = "The edited product was not in the database";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get the product and seed it
            var product = this.GetProduct();
            await this.SeedSingleProduct(context);

            EditProductBindingModel model = new EditProductBindingModel
            {
                Id = product.Id,
                Name = "NewName",
                Price = 12,
                Specification = "NewSpec",
                Description = "NewDesc",
            };

            var methodResult = await productService.EditProduct(model);

            Assert.True(methodResult, onFalseErrorMessage);

            var productFromDatabase = await context.Products.FirstOrDefaultAsync(
                p => p.Id == product.Id && p.Name == model.Name && p.Price == model.Price &&
                p.Specification == model.Specification && p.Description == model.Description);

            AssertExtensions.NotNullWithMessage(productFromDatabase, onNullErrorMessage);
        }

        [Fact]
        public async Task GetEditProductInfoById_WithExistingProduct_ShouldReturnItsData()
        {
            string onFalseErrorMessage = "The returned model was with different data than the actual product.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get the product and seed it
            var product = this.GetProduct();
            await this.SeedSingleProduct(context);

            var resultModel = await productService.GetEditProductInfoById(product.Id);

            Assert.True(
                resultModel.Name == product.Name && resultModel.Price == product.Price &&
                resultModel.Specification == product.Specification && resultModel.Description == product.Description,
                onFalseErrorMessage);
        }

        [Fact]
        public async Task GetEditProductInfoById_WithNonExistingProduct_ShouldReturnNull()
        {
            string onNotNullErrorMessage = "The returned model was not null upon given non-existing product Id.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get the product and do not seed it
            var product = this.GetProduct();

            var resultModel = await productService.GetEditProductInfoById(product.Id);

            AssertExtensions.NullWithMessage(resultModel, onNotNullErrorMessage);
        }

        [Fact]
        public async Task GetProducts_WithNullInput_ShouldReturnAllProductsTheSameOrder()
        {
            string onCountDifferenceErrorMessage = "The count of the returned products is not correct.";
            string onFalseErrorMessage = "The returned products are not correct.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get multiple products and seed the categories with the products
            var products = this.GetMultipleProducts();
            await this.SeedMultipleSubCategoriesAndCategories(context);
            await this.SeedMultipleProducts(context);

            string emptyString = string.Empty;

            var resultProducts = await productService.GetProducts(null, null, 0);

            // (They should be 2 because 2 products are recommended)
            var expectedProductsCount = 2;

            // Making sure the count of the returned products is correct before comparing them 
            AssertExtensions.EqualCountWithMessage(
                expectedProductsCount, resultProducts.Count(), onCountDifferenceErrorMessage);

            foreach (var prd in resultProducts)
            {
                // The 2 recommended product names
                Assert.True(
                    prd.Name == "thirdProductName" || prd.Name == "sixthProductName", onFalseErrorMessage);
            }
        }

        [Fact]
        public async Task GetProducts_WithGivenOnlySearchString_ShouldReturnAllProductsContainingTheSearchString()
        {
            string onFalseErrorMessage = "The returned product names dont match the search criteria.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get multiple products and seed the categories with the products
            var products = this.GetMultipleProducts();
            await this.SeedMultipleSubCategoriesAndCategories(context);
            await this.SeedMultipleProducts(context);

            // Should return products containing that words in their name
            string searchString = "first fifth";

            var resultProducts = await productService.GetProducts(searchString, null, 0);

            foreach (var prd in resultProducts)
            {
                Assert.True(
                    prd.Name == "firstProductName" || prd.Name == "secondProductName",
                    onFalseErrorMessage);
            }
        }

        [Fact]
        public async Task GetProducts_WithGivenOnlySortOption_ShouldReturnAllProductsSortedByTheirNameDescending()
        {
            string onFalseErrorMessage = "The returned products are not in the same order as expected.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get multiple products and seed the categories with the products
            var products = this.GetMultipleProducts();
            await this.SeedMultipleSubCategoriesAndCategories(context);
            await this.SeedMultipleProducts(context);

            // Should return products ordered by name descending
            int sortBy = 4;

            var resultProducts = await productService.GetProducts(null, null, sortBy);

            // Making the collections List to be able to get product by index
            // Orderring the elements in the expected collection to match the actual products by index
            var listExpectedProducts = products.OrderByDescending(x => x.Name).ToList();
            var listActualProducts = resultProducts.ToList();

            for (int i = 0; i < listActualProducts.Count; i++)
            {
                var currActualP = listActualProducts[i];
                var currExpectedP = listExpectedProducts[i];

                Assert.True(
                    currActualP.Name == currExpectedP.Name,
                    onFalseErrorMessage);
            }
        }

        [Fact]
        public async Task GetProducts_WithGivenOnlyCategory_ShouldReturnAllProductsInTheCategory()
        {
            string onCountDifferenceErrorMessage = "The returned products are not the same count as expected.";
            string onStringDifferenceErrorMessage = "The returned product name is not the same as the expected one.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get multiple products and seed the categories with the products
            var products = this.GetMultipleProducts();
            await this.SeedMultipleSubCategoriesAndCategories(context);
            await this.SeedMultipleProducts(context);

            // Should return products ordered by name descending
            string subCategoryId = "B-SubCategoryId-forFirst";

            var resultProducts = await productService.GetProducts(null, subCategoryId, 0);

            // The retuned collection should contain only one product in this sub-category
            int expectedCount = 1;

            AssertExtensions.EqualCountWithMessage(
                expectedCount, resultProducts.Count(), onCountDifferenceErrorMessage);

            var product = resultProducts.First();
            var expectedProductName = "secondProductName";

            AssertExtensions.EqualStringWithMessage(
                product.Name, expectedProductName, onStringDifferenceErrorMessage);
        }

        [Fact]
        public async Task GetProducts_WithGivenAllFilterOptions_ShouldReturnTheProductsMatchingAllTheGivenCriteria()
        {
            string onCountDifferenceErrorMessage = "The returned products are not with the same count as expected.";
            string onStringDifferenceErrorMessage = "The products sorting is wrong.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get multiple products and seed the categories with the products
            var products = this.GetMultipleProducts();
            await this.SeedMultipleSubCategoriesAndCategories(context);
            await this.SeedMultipleProducts(context);

            // Should return products ordered by name descending
            string searchString = "first";
            string subCategoryId = "A-SubCategoryId-forFirst";
            int sortBy = 4;

            var resultProducts = await productService.GetProducts(searchString, subCategoryId, sortBy);

            var expectedCount = 2;

            AssertExtensions.EqualCountWithMessage(
                expectedCount, resultProducts.Count(), onCountDifferenceErrorMessage);

            var expectedFirstProductName = "B-firstProductName";
            var firstActualProductName = resultProducts.First().Name;

            AssertExtensions.EqualStringWithMessage(
                expectedFirstProductName, firstActualProductName, onStringDifferenceErrorMessage);
        }

        [Fact]
        public async Task GetSearchedProducts_WithWordThatProductsHave_ShouldReturnTheCorrectProducts()
        {
            string onCountDifferenceErrorMessage = "The returned products are not with the same count as expected.";
            string onFalseErrorMessage = "The product names don't match the search string.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get multiple products and seed the categories with the products
            var products = this.GetMultipleProducts();
            await this.SeedMultipleSubCategoriesAndCategories(context);
            await this.SeedMultipleProducts(context);

            // Should return all products containing the symbol or word in the string
            string searchString = "B";

            var resultProducts = productService.GetSearchedProducts(searchString);

            var expectedCount = 2;

            AssertExtensions.EqualCountWithMessage(
                expectedCount, resultProducts.Count(), onCountDifferenceErrorMessage);

            foreach (var prd in resultProducts)
            {
                Assert.True(prd.Name.StartsWith(searchString), onFalseErrorMessage);
            }
        }

        [Fact]
        public async Task GetSearchedProducts_WithWordThatProductsDontHave_ShouldReturnEmptyCollection()
        {
            string onCountDifferenceErrorMessage = "The method returned products but no products were expected.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get multiple products and seed the categories with the products
            var products = this.GetMultipleProducts();
            await this.SeedMultipleSubCategoriesAndCategories(context);
            await this.SeedMultipleProducts(context);

            // Should return all products containing the symbol or word in the string (no products)
            string searchString = "qqqq";

            var resultProducts = productService.GetSearchedProducts(searchString);

            var expectedCount = 0;

            AssertExtensions.EqualCountWithMessage(
                expectedCount, resultProducts.Count(), onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task GetRecommendationProducts_WithSeededRecommendedProducts_ShouldReturnTheCorrectProducts()
        {
            string onCountDifferenceErrorMessage = "The returned products are not with the same count as expected.";
            string onFalseErrorMessage = "The returned products were not correct.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get multiple products and seed the categories with the products
            var products = this.GetMultipleProducts();
            await this.SeedMultipleSubCategoriesAndCategories(context);
            await this.SeedMultipleProducts(context);

            var resultProducts = await productService.GetRecommendationProducts();

            var expectedCount = 2;
            AssertExtensions.EqualCountWithMessage(
                expectedCount, resultProducts.Count(), onCountDifferenceErrorMessage);

            foreach (var prd in resultProducts)
            {
                Assert.True(
                    prd.Name == "thirdProductName" || prd.Name == "sixthProductName", onFalseErrorMessage);
            }
        }

        [Fact]
        public async Task GetRecommendationProducts_WithNoSeededRecommendedProducts_ShouldReturnTheCorrectProducts()
        {
            string onCountDifferenceErrorMessage = "The method does not return anempty collection.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Seed the categories without the products
            await this.SeedMultipleSubCategoriesAndCategories(context);

            var resultProducts = await productService.GetRecommendationProducts();

            var expectedCount = 0;

            AssertExtensions.EqualCountWithMessage(
                expectedCount, resultProducts.Count(), onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task GetProductForCart_WithExistingProductId_ShouldReturnTheCorrectProduct()
        {
            string onStringDifferenceErrorMessage = "The returned product name does not match the expected one.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get multiple products and seed the categories with the products
            var products = this.GetMultipleProducts();
            await this.SeedMultipleSubCategoriesAndCategories(context);
            await this.SeedMultipleProducts(context);

            string productId = "forthProductId";

            var resultProduct = await productService.GetProductForCart(productId);

            string expectedProductName = "forthProductName";

            AssertExtensions.EqualStringWithMessage(
                expectedProductName, resultProduct.Name, onStringDifferenceErrorMessage);
        }

        [Fact]
        public async Task GetProductForCart_WithNonExistingProductId_ShouldReturnNull()
        {
            string onNonNullErrorMessage = "The method does not return null upon non-existing product.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cloudinaryServiceMock = new Mock<ICloudinaryService>();

            var productService = new ProductService(context, cloudinaryServiceMock.Object);

            // Get multiple products and seed the categories with the products
            var products = this.GetMultipleProducts();
            await this.SeedMultipleSubCategoriesAndCategories(context);
            await this.SeedMultipleProducts(context);

            string productId = "fakeProductId";

            var resultProduct = await productService.GetProductForCart(productId);

            AssertExtensions.NullWithMessage(resultProduct, onNonNullErrorMessage);
        }
    }
}
