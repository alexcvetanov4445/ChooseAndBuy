namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.BindingModels.Addresses;
    using ChooseAndBuy.Web.ViewModels.Addresses;
    using Microsoft.EntityFrameworkCore;

    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext context;

        public AddressService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateAddress(AddressCreateBindingModel addressCreate, string userId)
        {
            var address = AutoMapper.Mapper.Map<Address>(addressCreate);
            address.ApplicationUserId = userId;

            await this.context.Addresses.AddAsync(address);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<AddressViewModel>> GetAllUserAddresses(string userId)
        {
            var addresses = await this.context.Addresses
                .Where(a => a.ApplicationUserId == userId)
                .ToListAsync();

            var addressesViewModel = AutoMapper.Mapper.Map<List<AddressViewModel>>(addresses);

            return addressesViewModel;
        }
    }
}
