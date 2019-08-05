namespace ChooseAndBuy.Web.ViewModels.Addresses
{
    using System.ComponentModel.DataAnnotations;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class AddressViewModel : IMapFrom<Address>
    {
        public string Id { get; set; }

        public string AddressText { get; set; }
    }
}
