namespace ChooseAndBuy.Data.Models
{
    using System;

    public class Address
    {
        public Address()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string AddressText { get; set; }

        public string CityId { get; set; }

        public City City { get; set; }
    }
}
