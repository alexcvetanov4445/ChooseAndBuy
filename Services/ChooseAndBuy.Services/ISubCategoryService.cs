namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.SubCategories;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ISubCategoryService
    {
        Task<IEnumerable<SelectListItem>> GetSubCategories();

        Task<bool> AddSubCategory(SubCategoryBindingModel model);

        Task<bool> SubCategoryExists(string name);
    }
}
