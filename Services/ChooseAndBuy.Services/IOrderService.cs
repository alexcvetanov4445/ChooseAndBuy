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
    }
}
