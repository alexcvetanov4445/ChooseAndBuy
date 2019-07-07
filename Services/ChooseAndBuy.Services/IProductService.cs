namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Products;

    public interface IProductService
    {
        bool ProductExists(string name);

        void AddProduct(Product product);

        bool DeleteProduct(string id);

        bool EditProduct(Product product);

        string GetIdByName(string productName);

        IEnumerable<Product> GetProducts(string search, string subCategoryId);

        IEnumerable<Product> GetAllProducts();

        Product GetById(string id);
    }
}
