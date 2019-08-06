namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChooseAndBuy.Web.ViewModels.Administration.Orders;
    using ChooseAndBuy.Web.ViewModels.Orders;

    public interface IOrderService
    {
        Task<string> CreateOrder(OrderBindingModel order);

        Task<bool> AddProductsToOrder(string orderId, List<OrderProductViewModel> products);

        Task<bool> ApproveOrder(string orderId);

        Task<bool> DeliverOrder(string orderId);

        Task<bool> ReturnOrder(string orderId, string returnReason);

        Task<bool> CancelOrder(string orderId);

        Task<ConfirmationViewModel> GetConfirmationInfo(string orderId);

        Task<IEnumerable<OrderViewModel>> GetAllUserOrders(string userId);

        Task<IEnumerable<AdminPaneOrderViewModel>> GetPendingOrders();

        Task<IEnumerable<AdminPaneOrderViewModel>> GetActiveOrders();
    }
}
