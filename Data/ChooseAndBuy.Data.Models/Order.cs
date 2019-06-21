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

        public decimal DeliveryPrice { get; set; }

        public PaymentType PaymentType { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }

        public string DeliveryAddressId { get; set; }

        public Address DeliveryAddress { get; set; }
    }
}
