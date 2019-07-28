namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChooseAndBuy.Web.BindingModels.Addresses;
    using ChooseAndBuy.Web.ViewModels.Addresses;

    public interface IAddressService
    {
        Task<bool> CreateAddress(AddressCreateBindingModel addressCreate, string userId);

        Task<IEnumerable<AddressViewModel>> GetAllUserAddresses(string userId);
    }
}
