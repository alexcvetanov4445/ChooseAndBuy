﻿namespace ChooseAndBuy.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.BindingModels.Administration.Categories;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : AdministrationController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.categoryService.AddCategory(model);

            this.TempData["Success"] = $"Successully created {model.Name} category.";

            return this.RedirectToAction("Create");
        }

        [HttpGet]
        public async Task<IActionResult> Deletable()
        {
            var deletableCategoriesModels = await this.categoryService.GetDeletableCategories();

            return this.View(deletableCategoriesModels);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await this.categoryService.DeleteCategory(id);

            return this.Json(result);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CategoryNameExists(string name)
        {
            var isAvailableName = await this.categoryService.ValidateCategoryName(name);

            return this.Json(isAvailableName);
        }
    }
}