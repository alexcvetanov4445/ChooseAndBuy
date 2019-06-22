namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class CnbUser : IdentityUser
    {
        public CnbUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Orders = new HashSet<Order>();
            this.Addresses = new HashSet<Address>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public string ShoppingCartId { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }

        // public bool IsDeleted { get; set; }
        // public DateTime? DeletedOn { get; set; }
    }
}
