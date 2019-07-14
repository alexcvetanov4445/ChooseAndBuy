namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Orders;

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;

        public OrderService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool CreateOrder(Order order)
        {
            this.context.Orders.Add(order);

            this.context.SaveChanges();

            return true;
        }

        public bool AddProductsToOrder(string orderId, List<OrderProductViewModel> products)
        {
            foreach (var product in products)
            {
                OrderProduct orderProduct = new OrderProduct
                {
                    OrderId = orderId,
                    Quantity = product.Quantity,
                    ProductId = product.Id,
                    Price = product.TotalPrice,
                };

                this.context.OrderProducts.Add(orderProduct);
            }

            this.context.SaveChanges();

            return true;
        }
    }
}
