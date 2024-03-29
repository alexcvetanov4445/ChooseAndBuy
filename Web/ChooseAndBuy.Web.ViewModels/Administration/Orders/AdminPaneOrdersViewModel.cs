﻿namespace ChooseAndBuy.Web.ViewModels.Administration.Orders
{
    using System.Collections.Generic;

    using ChooseAndBuy.Web.BindingModels.Orders;

    public class AdminPaneOrdersViewModel
    {
        public ICollection<AdminPaneOrderViewModel> Orders { get; set; }

        public ReturnReasonBindingModel ReasonModel { get; set; }
    }
}
