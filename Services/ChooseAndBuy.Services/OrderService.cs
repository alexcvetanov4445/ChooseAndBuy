namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Data.Models.Enums;
    using ChooseAndBuy.Web.ViewModels.Orders;
    using Microsoft.EntityFrameworkCore;

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

        public Order GetOrderById(string orderId)
        {
            var order = this.context.Orders
                .Include(o => o.DeliveryAddress)
                .ThenInclude(da => da.City)
                .SingleOrDefault(o => o.Id == orderId);

            return order;
        }

        public IEnumerable<Order> GetAllUserOrders(string userId)
        {
            var orders = this.context.Orders.Include(o => o.DeliveryAddress).Where(o => o.ApplicationUserId == userId);

            return orders;
        }

        public IEnumerable<Order> GetPendingOrders()
        {
            var orders = this.context.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.DeliveryAddress)
                .Where(o => o.Status == OrderStatus.Pending);

            return orders;
        }

        public IEnumerable<Order> GetActiveOrders()
        {
            var orders = this.context.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.DeliveryAddress)
                .Where(o => o.Status != OrderStatus.Pending);

            return orders;
        }

        public bool ApproveOrder(string orderId)
        {
            var order = this.context.Orders.SingleOrDefault(o => o.Id == orderId);

            order.Status = OrderStatus.DeliveryInProgress;
            this.context.SaveChanges();

            return true;
        }

        public bool DeliverOrder(string orderId)
        {
            var order = this.context.Orders.SingleOrDefault(o => o.Id == orderId);

            if (order.Status == OrderStatus.Canceled)
            {
                return false;
            }

            order.Status = OrderStatus.Delivered;
            order.DeliveryDate = DateTime.UtcNow;
            this.context.SaveChanges();

            return true;
        }

        public bool CancelOrder(string orderId)
        {
            var order = this.context.Orders.SingleOrDefault(o => o.Id == orderId);

            if (order.Status == OrderStatus.Delivered)
            {
                return false;
            }

            order.Status = OrderStatus.Canceled;
            this.context.SaveChanges();

            return true;
        }
    }
}
