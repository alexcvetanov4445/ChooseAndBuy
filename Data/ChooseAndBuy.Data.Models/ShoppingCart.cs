namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class ShoppingCart
    {
        public ShoppingCart()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ShoppingCartProducts = new HashSet<ShoppingCartProduct>();
        }

        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }
    }
}
