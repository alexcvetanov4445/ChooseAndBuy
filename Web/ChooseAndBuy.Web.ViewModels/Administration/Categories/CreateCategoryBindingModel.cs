namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;
    using Microsoft.AspNetCore.Mvc;

    public class CreateCategoryBindingModel : IMapTo<Category>
    {
        [Required]
        [StringLength(20, ErrorMessage = "The Category name must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Remote(action: "CategoryNameExists", controller: "Categories", ErrorMessage = "A Category with the same name exists!")]
        public string Name { get; set; }
    }
}