namespace ChooseAndBuy.Web.ViewModels.Administration.Orders
{
    using System.Collections.Generic;

    using ChooseAndBuy.Web.ViewModels.Orders;

    public class AdminPaneOrdersViewModel
    {
        public ICollection<AdminPaneOrderViewModel> Orders { get; set; }

        public ReturnReasonBindingModel ReasonModel { get; set; }
    }
}
