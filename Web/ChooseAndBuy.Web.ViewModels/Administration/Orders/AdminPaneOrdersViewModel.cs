namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Orders
{
    using System.Collections.Generic;

    using ChooseAndBuy.Web.ViewModels.Orders;

    public class AdminPaneOrdersViewModel
    {
        public ICollection<AdminPaneOrderViewModel> Orders { get; set; }

        public ReturnReasonBindingModel ReasonModel { get; set; }
    }
}
