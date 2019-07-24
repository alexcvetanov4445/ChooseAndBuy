namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ShoppingCartProducts = new HashSet<ShoppingCartProduct>();
            this.Reviews = new HashSet<Review>();
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsRecommended { get; set; }

        public bool IsHidden { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Specification { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public string ImageName { get; set; }

        public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
