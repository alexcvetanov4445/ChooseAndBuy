namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;

    using ChooseAndBuy.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICategoryService
    {
        void AddCategory(Category category);

        IEnumerable<SelectListItem> GetCategories();

        Dictionary<string, ICollection<SelectListItem>> GetCategoriesWithSubCategories();

        bool ValidateCategoryName(string name);
    }
}
