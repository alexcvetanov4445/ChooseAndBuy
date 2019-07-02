namespace ChooseAndBuy.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ProductDetailsViewModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string ImageName { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public ICollection<ProductReviewViewModel> Reviews { get; set; }

    }
}
