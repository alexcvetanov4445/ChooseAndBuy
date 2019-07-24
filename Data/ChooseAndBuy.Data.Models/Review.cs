namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        public Review()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        public string ClientFullName { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}
