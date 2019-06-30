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
        private readonly IMapper mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
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

            var category = this.mapper.Map<Category>(model);

            this.categoryService.AddCategory(category);

            this.TempData["Success"] = $"Successully created {model.Name} category.";

            return this.View(model);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CategoryNameExists(string name)
        {
            var isAvailableName = this.categoryService.ValidateCategoryName(name);

            if (isAvailableName == false)
            {
                return this.Json(false);
            }

            return this.Json(true);
        }
    }
}