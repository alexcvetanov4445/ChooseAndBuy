namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Images = new HashSet<Image>();
            this.ShoppingCartProducts = new HashSet<ShoppingCartProduct>();
            this.Reviews = new HashSet<Review>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public decimal Price { get; set; }

        public string SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
