namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data.Models.Enums;

    public class AdminPaneOrderViewModel
    {
        public string Id { get; set; }

        public string Address { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public string Username { get; set; }

        // details additions
        public ICollection<AdminPaneOrderProductModel> Products { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal TotalPrice { get; set; }

        public int Quantity { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}
