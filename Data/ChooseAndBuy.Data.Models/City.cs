namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class City
    {
        public City()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Addresses = new HashSet<Address>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Postcode { get; set; }

        public ICollection<Address> Addresses { get; set; }
    }
}
