namespace ChooseAndBuy.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Data.Models.Enums;

    public class OrderBindingModel
    {
        public string ApplicationUserId { get; set; }

        public decimal DeliveryFee { get; set; }

        [Required(ErrorMessage = "Please choose a delivery address.")]
        public string DeliveryAddressId { get; set; }

        public Address DeliveryAddress { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Payment Option")]
        public PaymentType PaymentType { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Delivery Type")]
        public DeliveryType DeliveryType { get; set; }

        [Display(Name = "Additional Info - not necessary")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "The field Additional info must be a text with minimum length of {2} and maximum length of {1}.")]
        public string AdditionalInfo { get; set; }
        
        public List<OrderProductViewModel> Products { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
