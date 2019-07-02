namespace ChooseAndBuy.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ProductsViewModel
    {
        // Model for Index page of ProductsController
        // <mainCategory, listOfSubCategories>
        public string Search { get; set; }

        public string SubCategoryId { get; set; }

        public Dictionary<string, ICollection<SelectListItem>> Categories { get; set; }

        public ICollection<ProductViewModel> Products { get; set; }
    }
}
