namespace ChooseAndBuy.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ChooseAndBuy.Web.BindingModels.Addresses;
    using ChooseAndBuy.Web.BindingModels.Orders;
    using ChooseAndBuy.Web.ViewModels.Addresses;
    using ChooseAndBuy.Web.ViewModels.Administration.Orders;

    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            this.OrderCreate = new OrderBindingModel();
        }

        public AddressCreateBindingModel AddressCreate { get; set; }

        public ICollection<AddressViewModel> UserAddresses { get; set; }

        public OrderBindingModel OrderCreate { get; set; }
    }
}
