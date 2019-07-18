namespace ChooseAndBuy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using X.PagedList;

    public class ProductsController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IProductService productsService;
        private readonly IReviewService reviewService;

        public ProductsController(
            ICategoryService categoryService,
            IProductService productsService,
            IReviewService reviewService)
        {
            this.categoryService = categoryService;
            this.productsService = productsService;
            this.reviewService = reviewService;
        }

        public async Task<IActionResult> Index(ProductsViewModel model)
        {
            // TODO: make the default values contstants
            var page = model.PageNum ?? 1;
            var show = model.ShowNum ?? 6;
            var sortBy = model.SortBy ?? 1;

            var products = await this.productsService.GetProducts(model.Search, model.SubCategoryId, sortBy);

            var pagedProducts = products.ToPagedList(page, show);

            model.Categories = await this.categoryService.GetCategoriesWithSubCategories();
            model.Products = pagedProducts;

            return this.View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var detailsInfo = await this.productsService.GetById(id);

            detailsInfo.Reviews = await this.reviewService.GetReviewsForProduct(id);

            AllDetailsViewModel productModel = new AllDetailsViewModel
            {
                DetailsInfo = detailsInfo,
                ReviewModel = new ReviewBindingModel { ProductId = id },
            };

            return this.View(productModel);
        }

        [HttpPost]
        public async Task<IActionResult> Details(ReviewBindingModel productModel)
        {
            await this.reviewService.AddReview(productModel);

            return this.RedirectToAction("Details", new { id = productModel.ProductId });
        }
    }
}
