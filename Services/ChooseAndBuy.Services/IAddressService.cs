namespace ChooseAndBuy.Services
{
    using ChooseAndBuy.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IAddressService
    {
        bool CreateAddress(Address address);

        IEnumerable<Address> GetAllUserAddresses(string userId);
    }
}
