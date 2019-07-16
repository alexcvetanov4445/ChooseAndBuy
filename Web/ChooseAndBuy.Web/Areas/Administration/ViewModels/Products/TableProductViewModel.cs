namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TableProductViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsRecommended { get; set; }

        public bool IsHidden { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public decimal Price { get; set; }

        public string SubCategoryName { get; set; }
    }
}
