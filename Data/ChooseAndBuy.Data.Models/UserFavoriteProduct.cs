namespace ChooseAndBuy.Data.Models
{
    public class UserFavoriteProduct
    {
        public string ProductId { get; set; }

        public Product Product { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
