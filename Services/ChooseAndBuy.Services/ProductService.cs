namespace ChooseAndBuy.Services
{
    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;

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
    }
}
