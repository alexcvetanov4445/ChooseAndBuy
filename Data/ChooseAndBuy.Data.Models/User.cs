namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Orders = new HashSet<Order>();
            this.Addresses = new HashSet<Address>();
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public int ShoppingCartId { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
