namespace ChooseAndBuy.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class UserOrderViewModel
    {
        public ICollection<OrderViewModel> Orders { get; set; }
    }
}
