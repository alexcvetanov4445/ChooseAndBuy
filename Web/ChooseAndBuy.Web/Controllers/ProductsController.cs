﻿namespace ChooseAndBuy.Web.Controllers
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

        public IActionResult Index(ProductsViewModel model)
        {
            // TODO: make the default values contstants
            var page = model.PageNum ?? 1;
            var show = model.ShowNum ?? 6;
            var sortBy = model.SortBy ?? 1;

            var products = this.productsService.GetProducts(model.Search, model.SubCategoryId, sortBy);
            var mappedProducts = this.mapper.Map<IList<ProductViewModel>>(products);
            var pagedProducts = mappedProducts.ToPagedList(page, show);

            model.Categories = this.categoryService.GetCategoriesWithSubCategories();
            model.Products = pagedProducts;

            // TODO: Fix the size of the images in products page
            return this.View(model);
        }

        public IActionResult Details(string id)
        {
            var product = this.productsService.GetById(id);
            var reviews = this.reviewService.GetReviewsForProduct(id).ToList();

            var detailsInfo = this.mapper.Map<ProductDetailsViewModel>(product);
            detailsInfo.Reviews = this.mapper.Map<List<ProductReviewViewModel>>(reviews);

            AllDetailsViewModel productModel = new AllDetailsViewModel
            {
                DetailsInfo = detailsInfo,
                ReviewModel = new ReviewBindingModel { ProductId = id },
            };

            return this.View(productModel);
        }

        [HttpPost]
        public IActionResult Details(ReviewBindingModel productModel)
        {

            Review review = this.mapper.Map<Review>(productModel);

            this.reviewService.AddReview(review);

            return this.RedirectToAction("Details", new { id = productModel.ProductId });
        }
    }
}