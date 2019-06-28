namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.ServiceModels;

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddCategory(Category category)
        {
            this.context.Categories.Add(category);

            this.context.SaveChanges();
        }

        public IEnumerable<CategoryServiceModel> GetCategories()
        {
            var result = this.context.Categories.Select(c => new CategoryServiceModel
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();

            return result;
        }

        public bool ValidateCategoryName(string name)
        {
            var result = this.context.Categories.FirstOrDefault(n => n.Name == name);

            if (result != null)
            {
                return false;
            }

            return true;
        }
    }
}
