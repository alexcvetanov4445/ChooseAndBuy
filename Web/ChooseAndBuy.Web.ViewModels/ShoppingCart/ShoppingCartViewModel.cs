namespace ChooseAndBuy.Web.ViewModels.ShoppingCart
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ShoppingCartViewModel
    {
        public ICollection<ShoppingCartProductViewModel> Products { get; set; }

        public ShoppingCartUpdateCountBindingModel UpdateCountProduct { get; set; }

        public double TotalPrice { get; set; }
    }
}
