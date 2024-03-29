﻿namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.BindingModels.Administration.SubCategories;
    using ChooseAndBuy.Web.ViewModels.Administration.SubCategories;
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
            var categoryExists = await this.CategoryExists(model.CategoryId);

            if (!categoryExists)
            {
                return false;
            }

            var subCategory = AutoMapper.Mapper.Map<SubCategory>(model);

            await this.context.SubCategories.AddAsync(subCategory);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteSubCategory(string id)
        {
            var subCategory = await this.context.SubCategories.FirstOrDefaultAsync(sc => sc.Id == id);

            if (subCategory == null)
            {
                return false;
            }

            this.context.Remove(subCategory);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<SubCategoryViewModel>> GetDeletableSubCategories()
        {
            var subCategories = this.context.SubCategories
                .Include(c => c.Category)
                .Include(p => p.Products)
                .Where(p => p.Products.Count == 0);

            var result = AutoMapper.Mapper.Map<List<SubCategoryViewModel>>(subCategories);

            return result;
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

        private async Task<bool> CategoryExists(string categoryId)
        {
            var doesExist = await this.context
                .Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (doesExist == null)
            {
                return false;
            }

            return true;
        }
    }
}
