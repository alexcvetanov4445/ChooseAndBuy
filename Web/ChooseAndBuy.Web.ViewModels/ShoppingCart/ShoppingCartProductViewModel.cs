namespace ChooseAndBuy.Web.ViewModels.ShoppingCart
{
    public class ShoppingCartProductViewModel
    {
        public string ImageName { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public double TotalPrice { get; set; }
    }
}
