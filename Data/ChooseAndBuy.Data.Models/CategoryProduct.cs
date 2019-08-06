namespace ChooseAndBuy.Data.Models
{
    public class CategoryProduct
    {
        public string ProductId { get; set; }

        public Product Product { get; set; }

        public string SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }
    }
}
