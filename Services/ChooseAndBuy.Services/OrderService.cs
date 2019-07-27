namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Common;
    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Data.Models.Enums;
    using ChooseAndBuy.Web.ViewModels.Administration.Orders;
    using ChooseAndBuy.Web.ViewModels.Orders;
    using Microsoft.EntityFrameworkCore;

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;

        public OrderService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<string> CreateOrder(OrderBindingModel orderBindingModel)
        {
            var order = AutoMapper.Mapper.Map<Order>(orderBindingModel);

            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            order.Quantity = orderBindingModel.Products.Sum(op => op.Quantity);

            order.DispatchDate = order.DeliveryType == DeliveryType.Express ?
                DateTime.UtcNow.AddDays(GlobalConstants.ExpressDeliveryDays) :
                DateTime.UtcNow.AddDays(GlobalConstants.NormalDeliveryDays);

            await this.context.Orders.AddAsync(order);

            await this.context.SaveChangesAsync();

            return order.Id;
        }

        public async Task<bool> AddProductsToOrder(string orderId, List<OrderProductViewModel> products)
        {
            foreach (var product in products)
            {
                var orderProduct = AutoMapper.Mapper.Map<OrderProduct>(product);

                orderProduct.OrderId = orderId;

                await this.context.OrderProducts.AddAsync(orderProduct);
            }

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<ConfirmationViewModel> GetConfirmationInfo(string orderId)
        {
            var order = await this.context.Orders
                .Include(o => o.DeliveryAddress)
                .ThenInclude(da => da.City)
                .SingleOrDefaultAsync(o => o.Id == orderId);

            var confirmationViewModel = AutoMapper.Mapper.Map<ConfirmationViewModel>(order);

            return confirmationViewModel;
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllUserOrders(string userId)
        {
            var orders = await this.context
                .Orders
                .Include(o => o.DeliveryAddress)
                .Where(o => o.ApplicationUserId == userId)
                .ToListAsync();

            var ordersViewModel = AutoMapper.Mapper.Map<List<OrderViewModel>>(orders);

            return ordersViewModel;
        }

        public async Task<IEnumerable<AdminPaneOrderViewModel>> GetPendingOrders()
        {
            var orders = await this.context.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.DeliveryAddress)
                .Include(o => o.OrderProducts)
                .ThenInclude(o => o.Product)
                .Where(o => o.Status == OrderStatus.Pending)
                .ToListAsync();

            var ordersViewModel = AutoMapper.Mapper.Map<List<AdminPaneOrderViewModel>>(orders);

            return ordersViewModel;
        }

        public async Task<IEnumerable<AdminPaneOrderViewModel>> GetActiveOrders()
        {
            var orders = await this.context.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.DeliveryAddress)
                .Include(o => o.OrderProducts)
                .ThenInclude(o => o.Product)
                .Where(o => o.Status != OrderStatus.Pending)
                .ToListAsync();

            var ordersViewModel = AutoMapper.Mapper.Map<List<AdminPaneOrderViewModel>>(orders);

            return ordersViewModel;
        }

        public async Task<bool> ApproveOrder(string orderId)
        {
            var order = await this.context.Orders.SingleOrDefaultAsync(o => o.Id == orderId);

            order.Status = OrderStatus.DeliveryInProgress;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeliverOrder(string orderId)
        {
            var order = await this.context.Orders.SingleOrDefaultAsync(o => o.Id == orderId);

            if (order.Status == OrderStatus.Canceled)
            {
                return false;
            }

            order.Status = OrderStatus.Delivered;
            order.DeliveryDate = DateTime.UtcNow;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CancelOrder(string orderId)
        {
            var order = await this.context.Orders.SingleOrDefaultAsync(o => o.Id == orderId);

            if (order.Status == OrderStatus.Delivered)
            {
                return false;
            }

            order.Status = OrderStatus.Canceled;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ReturnOrder(string orderId, string returnReason)
        {
            var order = await this.context.Orders.SingleOrDefaultAsync(o => o.Id == orderId);

            order.Status = OrderStatus.Returned;

            order.ReturnReason = returnReason;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
