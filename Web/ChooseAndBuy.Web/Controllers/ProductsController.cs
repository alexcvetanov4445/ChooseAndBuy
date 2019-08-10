namespace ChooseAndBuy.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Common;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.BindingModels.Products;
    using ChooseAndBuy.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> Index(ProductsViewModel model)
        {
            var page = model.PageNum ?? GlobalConstants.DefaultProductsPage;
            var show = model.ShowNum ?? GlobalConstants.DefaultProductsShow;
            var sortBy = model.SortBy ?? GlobalConstants.DefaultProductsSorting;

            var products = await this.productsService.GetProducts(model.Search, model.SubCategoryId, sortBy);

            var pagedProducts = products.ToPagedList(page, show);

            model.Categories = await this.categoryService.GetCategoriesWithSubCategories();
            model.Products = pagedProducts;

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            var products = this.productsService.GetSearchedProducts(term);

            if (products.Count() == 0)
            {
                return this.Json(new List<SearchViewModel>
                {
                    new SearchViewModel
                    {
                        Value = GlobalConstants.SearchNoResults,
                        Url = string.Empty,
                    },
                });
            }

            var result = products.Select(x => new SearchViewModel
            {
                Value = x.Name,
                Url = GlobalConstants.ProductDetailsUrl + x.Id,
            });

            return this.Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var detailsInfo = await this.productsService.GetById(id);

            detailsInfo.Reviews = await this.reviewService.GetReviewsForProduct(id);

            var recommendedProducts = await this.productsService.GetRecommendationProducts();

            AllDetailsViewModel productModel = new AllDetailsViewModel
            {
                DetailsInfo = detailsInfo,
                ReviewModel = new ReviewBindingModel { ProductId = id },
                RecommendedProducts = recommendedProducts.ToList(),
            };

            return this.View(productModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewBindingModel productModel)
        {
            await this.reviewService.AddReview(productModel);

            return this.RedirectToAction("Details", new { id = productModel.ProductId });
        }
    }
}
