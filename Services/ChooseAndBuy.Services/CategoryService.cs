namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.BindingModels.Administration.Categories;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddCategory(CreateCategoryBindingModel model)
        {
            var category = AutoMapper.Mapper.Map<Category>(model);

            await this.context.Categories.AddAsync(category);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<SelectListItem>> GetCategories()
        {
            var result = await this.context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.Name,
            })
            .ToListAsync();

            return result;
        }

        public async Task<Dictionary<string, ICollection<SelectListItem>>> GetCategoriesWithSubCategories()
        {
            var dict = new Dictionary<string, ICollection<SelectListItem>>();

            var categories = await this.context
                .Categories
                .Include(x => x.SubCategories)
                .ToListAsync();

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

        public async Task<bool> ValidateCategoryName(string name)
        {
            var result = await this.context.Categories.FirstOrDefaultAsync(n => n.Name == name);

            if (result != null)
            {
                return false;
            }

            return true;
        }
    }
}
