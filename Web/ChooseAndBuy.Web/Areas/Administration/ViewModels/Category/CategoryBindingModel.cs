namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Category
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryBindingModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The Category name must be at least {0} and at max {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }
    }
}
