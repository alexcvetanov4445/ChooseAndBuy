﻿namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.ServiceModels;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ISubCategoryService
    {
        IEnumerable<SelectListItem> GetSubCategories();

        void AddSubCategory(SubCategory subCategory);
    }
}
