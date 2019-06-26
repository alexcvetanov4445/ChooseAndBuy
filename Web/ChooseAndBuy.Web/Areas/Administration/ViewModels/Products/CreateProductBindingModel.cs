using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Products
{
    public class CreateProductBindingModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Полето\"{0}\" e задължително.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Полето \"{0}\" трябва да бъде текст с минимална дължина {2} и максимална дължина {1}.")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Specification")]
        public string Specification { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        [Range(1, int.MaxValue, ErrorMessage = "Полето \"{0}\" трябва да е число в диапазона от {1} до {2}")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        public int ChildCategoryId { get; set; }

        public ICollection<SelectListItem> ChildCategories { get; set; }

        [Display(Name = "Images")]
        [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
        public ICollection<IFormFile> FormImages { get; set; }
    }
}
