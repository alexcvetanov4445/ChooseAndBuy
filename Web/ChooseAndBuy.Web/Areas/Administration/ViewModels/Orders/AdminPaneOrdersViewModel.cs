namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AdminPaneOrdersViewModel
    {
        public ICollection<AdminPaneOrderViewModel> Orders { get; set; }
    }
}
