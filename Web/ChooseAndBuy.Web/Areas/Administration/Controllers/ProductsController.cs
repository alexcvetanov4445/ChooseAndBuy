﻿namespace ChooseAndBuy.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.BindingModels.Administration.Products;
    using ChooseAndBuy.Web.ViewModels.Administration.Products;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : AdministrationController
    {
        private readonly ISubCategoryService subCategoryService;
        private readonly IProductService productService;
        private readonly ICloudinaryService imageService;

        public ProductsController(
            ISubCategoryService subCategoryService,
            IProductService productService,
            ICloudinaryService imageService)
        {
            this.subCategoryService = subCategoryService;
            this.productService = productService;
            this.imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await this.productService.GetAllProducts();

            AllProductsViewModel model = new AllProductsViewModel { Products = products.ToList() };
            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await this.subCategoryService.GetSubCategories();

            var model = new CreateProductBindingModel { SubCategories = categories.ToList() };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductBindingModel model)
        {
            var subCategories = await this.subCategoryService.GetSubCategories();

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.productService.AddProduct(model);

            this.TempData["Success"] = $"Successully created {model.Name}";

            return this.RedirectToAction("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Hide(string productId)
        {
            var result = await this.productService.HideProduct(productId);

            return this.Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Recommend(string productId)
        {
            var result = await this.productService.RecommendProduct(productId);

            return this.Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string productId)
        {
            var model = await this.productService.GetEditProductInfoById(productId);

            model.Id = productId;

            var subCategories = await this.subCategoryService.GetSubCategories();
            model.SubCategories = subCategories.ToList();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductBindingModel model)
        {
            await this.productService.EditProduct(model);

            this.TempData["Success"] = $"Successully edited {model.Name}";

            return this.RedirectToAction("All");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> ValidateProductName([Bind(Prefix = "Name")]string productName)
        {
            bool productExists = await this.productService.ProductExists(productName);

            if (productExists == true)
            {
                return this.Json(false);
            }

            return this.Json(true);
        }
    }
}
