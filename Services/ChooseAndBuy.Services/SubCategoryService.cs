namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.SubCategories;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class SubCategoryService : ISubCategoryService
    {
        private readonly ApplicationDbContext context;

        public SubCategoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddSubCategory(SubCategoryBindingModel model)
        {
            var subCategory = AutoMapper.Mapper.Map<SubCategory>(model);

            await this.context.SubCategories.AddAsync(subCategory);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<SelectListItem>> GetSubCategories()
        {
            var categories = await this.context.SubCategories.Select(sc => new SelectListItem
            {
                Value = sc.Id,
                Text = sc.Name + $"({sc.Category.Name})",
            })
            .ToListAsync();

            return categories;
        }

        public async Task<bool> SubCategoryExists(string name)
        {
            return await this.context.SubCategories.AnyAsync(sc => sc.Name == name);
        }
    }
}
