namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChooseAndBuy.Web.BindingModels.Administration.Categories;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICategoryService
    {
        Task<bool> AddCategory(CreateCategoryBindingModel model);
        
        Task<IEnumerable<SelectListItem>> GetCategories();

        Task<Dictionary<string, ICollection<SelectListItem>>> GetCategoriesWithSubCategories();

        Task<bool> ValidateCategoryName(string name);
    }
}
