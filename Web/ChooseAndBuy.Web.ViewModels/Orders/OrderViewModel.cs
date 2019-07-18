namespace ChooseAndBuy.Web.ViewModels.Orders
{
    using System;

    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class OrderViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Address { get; set; }

        public DateTime ExpectedDelivery { get; set; }

        public string Status { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderViewModel>()
                .ForMember(model => model.ExpectedDelivery, opt => opt.MapFrom(src => src.DispatchDate.Value.ToShortDateString()))
                .ForMember(model => model.Address, opt => opt.MapFrom(src => src.DeliveryAddress.AddressText))
                .ForMember(model => model.Price, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(model => model.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}