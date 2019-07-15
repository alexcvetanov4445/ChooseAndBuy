namespace ChooseAndBuy.Web.ViewModels.Orders
{
    public class ConfirmationViewModel
    {
        public string ExpectedDelivery { get; set; }

        public string PaymentMethod { get; set; }

        public int QuantityProducts { get; set; }

        public decimal TotalPrice { get; set; }

        public string PhoneNumber { get; set; }

        public string ClientName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
    }
}
