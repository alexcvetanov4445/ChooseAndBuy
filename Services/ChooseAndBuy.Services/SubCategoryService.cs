namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.ServiceModels;

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

        public IEnumerable<SubCategoryServiceModel> GetSubCategories()
        {
            var categories = this.context.SubCategories.Select(sc => new SubCategoryServiceModel
            {
                Id = sc.Id,
                Name = sc.Name,
                MainCategory = sc.Category.Name,
            })
            .ToList();

            return categories;
        }
    }
}
