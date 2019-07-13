namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;

    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext context;

        public AddressService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool CreateAddress(Address address)
        {
            this.context.Addresses.Add(address);

            this.context.SaveChanges();

            return true;
        }

        public IEnumerable<Address> GetAllUserAddresses(string userId)
        {
            var addresses = this.context.Addresses
                .Where(a => a.ApplicationUserId == userId)
                .ToList();

            return addresses;
        }
    }
}
