namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CategoryProduct
    {
        public string ProductId { get; set; }

        public Product Product { get; set; }

        public string SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }
    }
}
