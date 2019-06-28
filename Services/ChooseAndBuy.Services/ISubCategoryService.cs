namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.ServiceModels;

    public interface ISubCategoryService
    {
        IEnumerable<SubCategoryServiceModel> GetSubCategories();

        void AddSubCategory(SubCategory subCategory);
    }
}
