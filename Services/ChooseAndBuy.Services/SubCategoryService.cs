namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SubCategoryService : ISubCategoryService
    {
        private readonly ApplicationDbContext context;

        public SubCategoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddSubCategory(SubCategory subCategory)
        {
            this.context.SubCategories.Add(subCategory);

            this.context.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetSubCategories()
        {
            var categories = this.context.SubCategories.Select(sc => new SelectListItem
            {
                Value = sc.Id,
                Text = sc.Name + $"({sc.Category.Name})",
            })
            .ToList();

            return categories;
        }
    }
}
