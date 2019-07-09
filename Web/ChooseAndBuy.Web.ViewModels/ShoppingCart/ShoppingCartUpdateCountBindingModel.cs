namespace ChooseAndBuy.Web.ViewModels.ShoppingCart
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class ShoppingCartUpdateCountBindingModel
    {
        public string ProductId { get; set; }

        [Range(typeof(int), "1", "12", ErrorMessage = "You can choose maximum of {2} products")]
        public int Quantity { get; set; }
    }
}
