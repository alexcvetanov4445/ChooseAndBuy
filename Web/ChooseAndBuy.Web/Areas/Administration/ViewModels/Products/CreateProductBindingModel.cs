namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateProductBindingModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must be a text with minimum legth of {2} and maximum length of {1}.")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Specification")]
        public string Specification { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The field \"{0}\" must be in the range between {1} and {2}")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        public int ChildCategoryId { get; set; }

        public ICollection<SelectListItem> ChildCategories { get; set; }

        [Display(Name = "Images")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        public ICollection<IFormFile> FormImages { get; set; }
    }
}
