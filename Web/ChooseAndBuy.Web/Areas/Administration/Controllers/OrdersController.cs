namespace ChooseAndBuy.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Orders;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : AdministrationController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(
            IOrderService orderService,
            IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        public IActionResult PendingOrders()
        {
            var orders = this.orderService.GetPendingOrders();

            var mappedOrders = this.mapper.Map<List<AdminPaneOrderViewModel>>(orders);

            AdminPaneOrdersViewModel model = new AdminPaneOrdersViewModel
            {
                Orders = mappedOrders,
            };

            return this.View(model);
        }

        public IActionResult ActiveOrders()
        {
            var orders = this.orderService.GetActiveOrders();

            var mappedOrders = this.mapper.Map<List<AdminPaneOrderViewModel>>(orders);

            AdminPaneOrdersViewModel model = new AdminPaneOrdersViewModel
            {
                Orders = mappedOrders,
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Approve(string orderId)
        {
            var result = this.orderService.ApproveOrder(orderId);

            return this.Json(result);
        }

        [HttpPost]
        public IActionResult Deliver(string orderId)
        {
            var orderIdCut = orderId.Remove(0, 14);

            var result = this.orderService.DeliverOrder(orderIdCut);

            return this.Json(result);
        }

        [HttpPost]
        public IActionResult Cancel(string orderId)
        {
            var orderIdCut = orderId.Remove(0, 13);

            var result = this.orderService.CancelOrder(orderIdCut);

            return this.Json(result);
        }
    }
}