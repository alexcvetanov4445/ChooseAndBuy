namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.BindingModels.Products;
    using ChooseAndBuy.Web.ViewModels.Products;

    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext context;

        public ReviewService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddReview(ReviewBindingModel productModel)
        {
            var review = AutoMapper.Mapper.Map<Review>(productModel);

            await this.context.Reviews.AddAsync(review);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<ICollection<ProductReviewViewModel>> GetReviewsForProduct(string productId)
        {
            var reviews = this.context.Reviews.Where(pr => pr.ProductId == productId);

            var result = AutoMapper.Mapper.Map<List<ProductReviewViewModel>>(reviews);

            return result;
        }
    }
}
