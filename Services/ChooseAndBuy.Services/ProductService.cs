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

        public bool ProductExists(string name)
        {
            var result = this.context.Products.SingleOrDefault(n => n.Name == name);

            if (result == null)
            {
                return false;
            }

            return true;
        }
    }
}
