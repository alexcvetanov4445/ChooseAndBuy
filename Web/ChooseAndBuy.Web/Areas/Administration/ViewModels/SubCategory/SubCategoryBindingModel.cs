namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.SubCategory
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class SubCategoryBindingModel
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please, select a {0}.")]
        [Display(Name = "Category")]
        public string ParentId { get; set; }

        public ICollection<ChooseAndBuy.Data.Models.Category> ParentCategories { get; set; }
    }
}
