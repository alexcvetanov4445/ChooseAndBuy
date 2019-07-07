namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AllProductsViewModel
    {
        public ICollection<TableProductViewModel> Products { get; set; }
    }
}
