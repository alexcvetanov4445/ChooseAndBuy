namespace ChooseAndBuy.Web.ViewModels.Addresses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class AddressViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "The field \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        public string AddressText { get; set; }
    }
}
