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
        private readonly IReviewService reviewService;
        private readonly IMapper mapper;

        public ProductsController(
            ICategoryService categoryService,
            IProductService productsService,
            IReviewService reviewService,
            IMapper mapper)
        {
            this.categoryService = categoryService;
            this.productsService = productsService;
            this.reviewService = reviewService;
            this.mapper = mapper;
        }

        public IActionResult Index(ProductsViewModel model, string id)
        {
            var products = this.productsService.GetProducts(model.Search, id);
            model.Categories = this.categoryService.GetCategoriesWithSubCategories();

            model.Products = this.mapper.Map<List<ProductViewModel>>(products);

            // TODO: Fix the size of the images in products page
            return this.View(model);
        }

        public IActionResult Details(string id)
        {
            var product = this.productsService.GetById(id);
            var reviews = this.reviewService.GetReviewsForProduct(id);

            var model = this.mapper.Map<ProductDetailsViewModel>(product);
            model.Reviews = this.mapper.Map<List<ProductReviewViewModel>>(reviews);

            return this.View(model);
        }

        // TODO: MAKE METHOD FOR AddReview (separate from details)
    }
}
