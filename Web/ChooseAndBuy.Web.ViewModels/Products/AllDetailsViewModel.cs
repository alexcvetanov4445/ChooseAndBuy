namespace ChooseAndBuy.Web.ViewModels.Products
{
    using ChooseAndBuy.Web.BindingModels.Products;
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
