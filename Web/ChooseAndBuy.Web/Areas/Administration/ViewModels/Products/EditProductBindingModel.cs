namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditProductBindingModel
    {
        public string Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [MaxLength(300, ErrorMessage = "The \"{0}\" should be maximum of {1} characters long.")]
        public string Description { get; set; }

        [Display(Name = "Specification")]
        [MaxLength(300, ErrorMessage = "The \"{0}\" should be maximum of {1} characters long.")]
        public string Specification { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The field \"{0}\" must be in the range between {1} and {2}")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        public string SubCategoryId { get; set; }

        public ICollection<SelectListItem> SubCategories { get; set; }
    }
}
