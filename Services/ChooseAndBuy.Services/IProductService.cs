namespace ChooseAndBuy.Services
{
    using ChooseAndBuy.Data.Models;

    public interface IProductService
    {
        bool ProductExists(string name);

        void AddProduct(Product product);

        string GetIdByName(string productName);
    }
}
