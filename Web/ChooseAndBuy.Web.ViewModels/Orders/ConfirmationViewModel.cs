namespace ChooseAndBuy.Web.ViewModels.Orders
{
    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class ConfirmationViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string ExpectedDelivery { get; set; }

        public string PaymentMethod { get; set; }

        public int QuantityProducts { get; set; }

        public decimal TotalPrice { get; set; }

        public string PhoneNumber { get; set; }

        public string ClientName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, ConfirmationViewModel>()
                .ForMember(m => m.ExpectedDelivery, opt => opt.MapFrom(src => src.DispatchDate.Value.ToShortDateString()))
                .ForMember(m => m.PaymentMethod, opt => opt.MapFrom(src => src.PaymentType))
                .ForMember(m => m.QuantityProducts, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(m => m.PhoneNumber, opt => opt.MapFrom(src => src.DeliveryAddress.PhoneNumber))
                .ForMember(m => m.ClientName, opt => opt.MapFrom(src => src.DeliveryAddress.FirstName + " " + src.DeliveryAddress.LastName))
                .ForMember(m => m.Address, opt => opt.MapFrom(src => src.DeliveryAddress.AddressText))
                .ForMember(m => m.City, opt => opt.MapFrom(src => src.DeliveryAddress.City.Name));
        }
    }
}
