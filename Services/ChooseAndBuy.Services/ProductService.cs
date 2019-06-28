namespace ChooseAndBuy.Services
{
    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
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
    }
}
