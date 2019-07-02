namespace ChooseAndBuy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;

    public class ProductsController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IProductService productsService;
        private readonly IMapper mapper;

        public ProductsController(
            ICategoryService categoryService,
            IProductService productsService,
            IMapper mapper)
        {
            this.categoryService = categoryService;
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public IActionResult Index(ProductsViewModel model, string id)
        {
            var products = this.productsService.GetProducts(model.Search, id);

            // load all categories with subcategories and display them with model
            model.Categories = this.categoryService.GetCategoriesWithSubCategories();

            model.Products = this.mapper.Map<List<ProductViewModel>>(products);

            // TODO: Fix the size of the images in products page
            return this.View(model);
        }

        public IActionResult Details(string id)
        {
            // get product with the id from productsService, make the view dynamic

            return this.View();
        }
    }
}
