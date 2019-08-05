﻿namespace ChooseAndBuy.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.BindingModels.Administration.SubCategories;
    using Microsoft.AspNetCore.Mvc;

    public class SubCategoriesController : AdministrationController
    {
        private readonly ICategoryService categoryService;
        private readonly ISubCategoryService subCategoryService;

        public SubCategoriesController(
            ICategoryService categoryService,
            ISubCategoryService subCategoryService)
        {
            this.categoryService = categoryService;
            this.subCategoryService = subCategoryService;
        }

        public async Task<IActionResult> Create()
        {
            var categories = await this.categoryService.GetCategories();

            var model = new SubCategoryBindingModel { Categories = categories.ToList() };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubCategoryBindingModel model)
        {
            var categories = await this.categoryService.GetCategories();
            model.Categories = categories.ToList();

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.subCategoryService.AddSubCategory(model);

            this.TempData["Success"] = $"Successully created {model.Name} sub-category.";

            var emptyModel = new SubCategoryBindingModel { Categories = categories.ToList() };

            return this.View(emptyModel);
        }

        public async Task<IActionResult> ValidateSubCategoryName (string name)
        {
            var subCategoryExists = await this.subCategoryService.SubCategoryExists(name);

            if (subCategoryExists)
            {
                return this.Json(false);
            }

            return this.Json(true);
        }
    }
}