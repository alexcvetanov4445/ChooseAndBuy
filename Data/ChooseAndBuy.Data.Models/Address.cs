namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Address
    {
        public Address()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Orders = new HashSet<Order>();
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string AddressText { get; set; }

        public string CityId { get; set; }

        public City City { get; set; }

        public string CnbUserId { get; set; }

        public CnbUser CnbUser { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
