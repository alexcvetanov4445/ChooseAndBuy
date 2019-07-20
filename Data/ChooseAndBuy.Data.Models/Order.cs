namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.Collections.Generic;

    using ChooseAndBuy.Data.Models.Enums;

    public class Order
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.OrderProducts = new HashSet<OrderProduct>();
        }

        public string Id { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime? DispatchDate { get; set; }

        public decimal TotalPrice { get; set; }

        public int Quantity { get; set; }

        public string AdditionalInfo { get; set; }

        public string ReturnReason { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public PaymentType PaymentType { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }

        public string DeliveryAddressId { get; set; }

        public Address DeliveryAddress { get; set; }
    }
}
