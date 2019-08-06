namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChooseAndBuy.Web.BindingModels.Products;
    using ChooseAndBuy.Web.ViewModels.Products;

    public interface IReviewService
    {
        Task<ICollection<ProductReviewViewModel>> GetReviewsForProduct(string productId);

        Task<bool> AddReview(ReviewBindingModel productModel);
    }
}
