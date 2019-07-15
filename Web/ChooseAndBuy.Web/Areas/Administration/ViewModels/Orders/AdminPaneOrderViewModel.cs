namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AdminPaneOrderViewModel
    {
        public string Id { get; set; }

        public string Address { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public string Username { get; set; }
    }
}
