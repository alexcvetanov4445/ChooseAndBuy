namespace ChooseAndBuy.Web.ViewModels.Products
{
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class ProductReviewViewModel : IMapFrom<Review>, IMapTo<Review>
    {
        public string ClientFullName { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
