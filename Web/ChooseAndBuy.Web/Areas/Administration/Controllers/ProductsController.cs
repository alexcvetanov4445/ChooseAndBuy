namespace ChooseAndBuy.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ProductsController : AdministrationController
    {
        private readonly ISubCategoryService subCategoryService;
        private readonly IProductService productService;

        private readonly IMapper mapper;

        public ProductsController(
            ISubCategoryService subCategoryService,
            IMapper mapper,
            IProductService productService)
        {
            this.subCategoryService = subCategoryService;
            this.mapper = mapper;
            this.productService = productService;
        }


        public IActionResult Create()
        {

            var categories = this.GetCategories();

            var model = new CreateProductBindingModel { SubCategories = categories };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.SubCategories = this.GetCategories();

                return this.View(model);
            }

            Product product = this.mapper.Map<Product>(model);

            this.productService.AddProduct(product);

            string productId = this.productService.GetIdByName(product.Name);

            // TODO: upload images from the collection in the model into image entity using product id for relation
            return this.Redirect("/Administration/Home/Index");
        }

        public List<SelectListItem> GetCategories()
        {
            var categories = this.subCategoryService
                .GetSubCategories()
                .Select(sc => new SelectListItem
                {
                    Value = sc.Id,
                    Text = $"{sc.Name} ({sc.MainCategory})",
                })
                .ToList();

            return categories;
        }
    }
}