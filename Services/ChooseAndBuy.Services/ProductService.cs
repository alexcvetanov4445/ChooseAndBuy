namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Products;
    using Microsoft.EntityFrameworkCore;

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

        public bool DeleteProduct(string id)
        {
            var product = this.context.Products.SingleOrDefault(x => x.Id == id);

            if (product == null)
            {
                return false;
            }

            this.context.Products.Remove(product);
            this.context.SaveChanges();

            return true;
        }

        public bool EditProduct(Product product)
        {
            this.context.Products.Update(product);
            this.context.SaveChanges();

            return true;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return this.context.Products.Include(p => p.SubCategory);
        }

        public Product GetById(string id)
        {
            var product = this.context.Products
                .Include(r => r.Reviews)
                .Include(c => c.SubCategory)
                .SingleOrDefault(pr => pr.Id == id);

            return product;
        }

        public string GetIdByName(string productName)
        {
            var name = this.context.Products.SingleOrDefault(x => x.Name == productName).Name;

            return name;
        }

        public IEnumerable<Product> GetProducts(string search, string subCategoryId, int sortBy)
        {
            var products = new List<Product>();

            if (search != null)
            {
                // get searched products
            }


            if (subCategoryId != null)
            {
                products = this.GetProductsByCategory(subCategoryId).ToList();
            }
            else
            {
                products = this.context.Products.Where(pr => pr.IsRecommended == true).ToList();
            }


            switch (sortBy)
            {
                case 1: // price ascending
                    return products.OrderBy(p => p.Price).ToList();
                case 2: // price descending
                    return products.OrderByDescending(p => p.Price).ToList();
                case 3: // name ascending
                    return products.OrderBy(p => p.Name).ToList();
                case 4: // name descending
                    return products.OrderByDescending(p => p.Name).ToList();
            }

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
