namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.ServiceModels;

    public interface ICategoryService
    {
        void AddCategory(Category category);

        IEnumerable<CategoryServiceModel> GetCategories();

        bool ValidateCategoryName(string name);
    }
}
