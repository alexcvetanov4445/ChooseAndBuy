namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Products;

    public interface IProductService
    {
        bool ProductExists(string name);

        void AddProduct(Product product);

        bool HideProduct(string id);

        bool RecommendProduct(string íd);

        bool EditProduct(Product product);

        string GetIdByName(string productName);

        IEnumerable<Product> GetProducts(string search, string subCategoryId, int sortBy);

        IEnumerable<Product> GetAllProducts();

        Product GetById(string id);
    }
}
