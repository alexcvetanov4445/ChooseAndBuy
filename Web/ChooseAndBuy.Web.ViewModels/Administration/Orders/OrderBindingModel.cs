namespace ChooseAndBuy.Web.ViewModels.Administration.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Data.Models.Enums;
    using ChooseAndBuy.Services.Mapping;
    using ChooseAndBuy.Web.BindingModels.Attributes;
    using ChooseAndBuy.Web.ViewModels.Orders;

    public class OrderBindingModel : IMapTo<Order>
    {
        public string ApplicationUserId { get; set; }

        public decimal DeliveryFee { get; set; }

        [Required(ErrorMessage = "Please choose a delivery address.")]
        public string DeliveryAddressId { get; set; }

        [Required(ErrorMessage = "Please choose a delivery address.")]
        public Address DeliveryAddress { get; set; }

        private int DefaultSelect = 0;

        [Display(Name = "Payment Option")]
        [Required(ErrorMessage = "This field is required")]
        [DropdownRequired]
        public PaymentType PaymentType { get; set; }

        [Display(Name = "Delivery Type")]
        [DropdownRequired]
        public DeliveryType DeliveryType { get; set; }

        [Display(Name = "Additional Info - not necessary")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "The field Additional info must be a text with minimum length of {2} and maximum length of {1}.")]
        public string AdditionalInfo { get; set; }

        public List<OrderProductViewModel> Products { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
