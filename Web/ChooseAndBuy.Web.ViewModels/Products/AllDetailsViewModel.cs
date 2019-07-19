namespace ChooseAndBuy.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AllDetailsViewModel
    {
        public ICollection<RecommendedProductViewModel> RecommendedProducts { get; set; }

        public ProductDetailsViewModel DetailsInfo { get; set; }

        public ReviewBindingModel ReviewModel { get; set; }
    }
}
