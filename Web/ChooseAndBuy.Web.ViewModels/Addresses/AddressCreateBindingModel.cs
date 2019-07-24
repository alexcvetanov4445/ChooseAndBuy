namespace ChooseAndBuy.Web.ViewModels.Addresses
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AddressCreateBindingModel : IMapTo<Address>
    {
        [Required(ErrorMessage = "{0} cannot be empty")]
        [Display(Name = "First Name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]

        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} cannot be empty.")]
        [Display(Name = "Last Name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} cannot be empty.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} cannot be empty.")]
        [Display(Name = "Address")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "The field \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        public string AddressText { get; set; }

        [Required(ErrorMessage = "Please choose a city.")]
        [Display(Name = "Choose a city")]
        public string CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }


    }
}
