namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.SubCategories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SubCategoryBindingModel
    {
        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please, select a {0}.")]
        public string CategoryId { get; set; }

        [Display(Name = "Category")]
        public ICollection<SelectListItem> Categories { get; set; }
    }
}
