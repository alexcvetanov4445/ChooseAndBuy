namespace ChooseAndBuy.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using ChooseAndBuy.Web.BindingModels.Products;

    public class AllDetailsViewModel
    {
        public ICollection<RecommendedProductViewModel> RecommendedProducts { get; set; }

        public ProductDetailsViewModel DetailsInfo { get; set; }

        public ReviewBindingModel ReviewModel { get; set; }
    }
}
