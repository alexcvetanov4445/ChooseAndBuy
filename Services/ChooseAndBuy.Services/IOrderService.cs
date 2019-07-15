namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Orders;

    public interface IOrderService
    {
        bool CreateOrder(Order order);

        bool AddProductsToOrder(string orderId, List<OrderProductViewModel> products);

        bool ApproveOrder(string orderId);

        bool DeliverOrder(string orderId);

        bool CancelOrder(string orderId);

        Order GetOrderById(string orderId);

        IEnumerable<Order> GetAllUserOrders(string userId);

        IEnumerable<Order> GetPendingOrders();

        IEnumerable<Order> GetActiveOrders();
    }
}
