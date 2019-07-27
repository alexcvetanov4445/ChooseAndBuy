namespace ChooseAndBuy.Web.ViewModels.Administration.Products
{
    using System.Collections.Generic;

    public class AllProductsViewModel
    {
        public ICollection<TableProductViewModel> Products { get; set; }
    }
}
