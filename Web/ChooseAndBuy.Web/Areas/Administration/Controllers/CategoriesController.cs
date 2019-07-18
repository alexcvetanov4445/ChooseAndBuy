namespace ChooseAndBuy.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Categories;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : AdministrationController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

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

            return this.View(model);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CategoryNameExists(string name)
        {
            var isAvailableName = await this.categoryService.ValidateCategoryName(name);

            return this.Json(isAvailableName);
        }
    }
}