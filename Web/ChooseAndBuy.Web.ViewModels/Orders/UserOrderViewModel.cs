namespace ChooseAndBuy.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserOrderViewModel
    {
        public ICollection<OrderViewModel> Orders { get; set; }
    }
}
