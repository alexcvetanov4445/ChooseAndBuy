namespace ChooseAndBuy.Data.Models
{
    using System;

    public class Review
    {
        public Review()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string ClientFullName { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}
