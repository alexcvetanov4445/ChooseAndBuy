namespace ChooseAndBuy.Web.ViewModels.ShoppingCart
{
    using System.Collections.Generic;

    using ChooseAndBuy.Web.BindingModels.ShoppingCart;

    public class ShoppingCartViewModel
    {
        public ICollection<ShoppingCartProductViewModel> Products { get; set; }

        public ShoppingCartUpdateCountBindingModel UpdateCountProduct { get; set; }

        public double TotalPrice { get; set; }
    }
}
