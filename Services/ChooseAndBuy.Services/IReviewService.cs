namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Products;

    public interface IReviewService
    {
        Task<ICollection<ProductReviewViewModel>> GetReviewsForProduct(string productId);

        Task<bool> AddReview(ReviewBindingModel productModel);
    }
}
