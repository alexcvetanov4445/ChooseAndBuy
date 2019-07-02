namespace ChooseAndBuy.Services
{
    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Products;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;

        public ProductService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddProduct(Product product)
        {
            this.context.Products.Add(product);

            this.context.SaveChanges();
        }

        public string GetIdByName(string productName)
        {
            var name = this.context.Products.SingleOrDefault(x => x.Name == productName).Name;

            return name;
        }

        public IEnumerable<Product> GetProducts(string search, string subCategoryId)
        {
            if (search != null)
            {
                // get searched products
            }

            if (subCategoryId != null)
            {
                return this.GetProductsByCategory(subCategoryId);
            }

            var products = this.context.Products.Where(pr => pr.IsRecommended == true);

            return products;
        }

        public bool ProductExists(string name)
        {
            var result = this.context.Products.SingleOrDefault(n => n.Name == name);

            if (result == null)
            {
                return false;
            }

            return true;
        }

        private IEnumerable<Product> GetProductsByCategory(string id)
        {
            var products = this.context.Products.Where(sc => sc.SubCategoryId == id);

            return products;
        }

    }
}
