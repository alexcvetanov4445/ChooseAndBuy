namespace ChooseAndBuy.Web.ViewModels.Administration.SubCategories
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SubCategoryBindingModel : IMapTo<SubCategory>
    {
        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [Remote(action: "ValidateSubCategoryName", controller: "SubCategories", ErrorMessage = "Sub-category name already exists!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please, select a {0}.")]
        public string CategoryId { get; set; }

        [Display(Name = "Category")]
        public ICollection<SelectListItem> Categories { get; set; }
    }
}
