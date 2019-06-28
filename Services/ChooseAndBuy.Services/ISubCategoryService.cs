namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;

    using ChooseAndBuy.Services.ServiceModels;

    public interface ISubCategoryService
    {
        IEnumerable<SubCategoryServiceModel> GetSubCategories();
    }
}
