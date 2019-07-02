namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<SelectListItem> GetCategories()
        {
            var result = this.context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.Name,
            })
            .ToList();

            return result;
        }

        public Dictionary<string, ICollection<SelectListItem>> GetCategoriesWithSubCategories()
        {
            var dict = new Dictionary<string, ICollection<SelectListItem>>();

            var categories = this.context.Categories.Include(x => x.SubCategories).ToList();

            foreach (var ctg in categories)
            {
                dict.Add(ctg.Name, new List<SelectListItem>());

                foreach (var subctg in ctg.SubCategories)
                {
                    dict[ctg.Name].Add(new SelectListItem
                    {
                        Text = subctg.Name,
                        Value = subctg.Id,
                    });
                }
            }

            return dict;
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
