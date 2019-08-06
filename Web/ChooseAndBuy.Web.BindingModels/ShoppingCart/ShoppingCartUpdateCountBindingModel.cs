namespace ChooseAndBuy.Web.BindingModels.ShoppingCart
{
    using System.ComponentModel.DataAnnotations;

    public class ShoppingCartUpdateCountBindingModel
    {
        public string ProductId { get; set; }

        [Range(typeof(int), "1", "12", ErrorMessage = "You can choose maximum of {2} products")]
        public int Quantity { get; set; }
    }
}
