﻿namespace ChooseAndBuy.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.BindingModels.Orders;
    using ChooseAndBuy.Web.ViewModels.Administration.Orders;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : AdministrationController
    {
        private readonly IOrderService orderService;

        public OrdersController(
            IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> PendingOrders()
        {
            var orders = await this.orderService.GetPendingOrders();

            AdminPaneOrdersViewModel model = new AdminPaneOrdersViewModel
            {
                Orders = orders.ToList(),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ActiveOrders()
        {
            var orders = await this.orderService.GetActiveOrders();

            AdminPaneOrdersViewModel model = new AdminPaneOrdersViewModel
            {
                Orders = orders.ToList(),
                ReasonModel = new ReturnReasonBindingModel(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Return(ReturnReasonBindingModel reasonModel)
        {
            var result = await this.orderService.ReturnOrder(reasonModel.OrderId, reasonModel.ReturnReason);

            return this.RedirectToAction("ActiveOrders");
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string orderId)
        {
            var result = await this.orderService.ApproveOrder(orderId);

            return this.Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Deliver(string orderId)
        {
            var orderIdCut = orderId;

            var result = await this.orderService.DeliverOrder(orderIdCut);

            return this.Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(string orderId)
        {
            var orderIdCut = orderId;

            var result = await this.orderService.CancelOrder(orderIdCut);

            return this.Json(result);
        }
    }
}
