namespace ChooseAndBuy.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OrderViewModel
    {
        public string Id { get; set; }

        public string Address { get; set; }

        public DateTime ExpectedDelivery { get; set; }

        public string Status { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}