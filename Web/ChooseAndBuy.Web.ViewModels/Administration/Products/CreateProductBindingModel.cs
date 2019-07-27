namespace ChooseAndBuy.Web.ViewModels.Administration.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateProductBindingModel : IMapTo<Product>
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        [Remote(action: "ValidateProductName", controller: "Products", ErrorMessage = "Product name already exists!")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [StringLength(400, MinimumLength = 20, ErrorMessage = "The field \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        public string Description { get; set; }

        [Display(Name = "Specification")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [StringLength(400, MinimumLength = 20, ErrorMessage = "The field \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        public string Specification { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The field \"{0}\" must be in the range between {1} and {2}")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        public string SubCategoryId { get; set; }

        public ICollection<SelectListItem> SubCategories { get; set; }

        [Display(Name = "Image")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        public IFormFile FormImage { get; set; }
    }
}
