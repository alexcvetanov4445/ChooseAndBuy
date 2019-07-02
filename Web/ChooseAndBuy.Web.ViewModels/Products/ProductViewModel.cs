namespace ChooseAndBuy.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ProductViewModel
    {
        // Model for a single product
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageName { get; set; }
    }
}
