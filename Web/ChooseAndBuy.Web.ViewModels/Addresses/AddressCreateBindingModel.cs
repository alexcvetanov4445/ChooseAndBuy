namespace ChooseAndBuy.Web.ViewModels.Addresses
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ChooseAndBuy.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AddressCreateBindingModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]

        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "The field \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        public string AddressText { get; set; }

        [Required]
        [Display(Name = "Choose a city")]
        public string CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }


    }
}
