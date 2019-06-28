namespace ChooseAndBuy.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.SubCategories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Create()
        {
            var categories = this.GetCategories();

            var model = new SubCategoryBindingModel { Categories = categories };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubCategoryBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = this.GetCategories();
                return this.View(model);
            }

            SubCategory subCategory = new SubCategory
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
            };

            this.subCategoryService.AddSubCategory(subCategory);

            return this.Redirect("/Administration/Home/Index");
        }

        private List<SelectListItem> GetCategories()
        {
            var categories = this.categoryService
                                 .GetCategories()
                                 .Select(c => new SelectListItem
                                 {
                                     Value = c.Id,
                                     Text = c.Name,
                                 })
                                 .ToList();

            return categories;
        }
    }
}