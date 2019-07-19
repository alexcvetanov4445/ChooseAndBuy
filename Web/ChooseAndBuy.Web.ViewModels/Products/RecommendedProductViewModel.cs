namespace ChooseAndBuy.Web.ViewModels.Products
{
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class RecommendedProductViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageName { get; set; }
    }
}
