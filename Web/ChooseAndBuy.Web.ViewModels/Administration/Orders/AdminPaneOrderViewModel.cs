namespace ChooseAndBuy.Web.Areas.Administration.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Data.Models.Enums;
    using ChooseAndBuy.Services.Mapping;

    public class AdminPaneOrderViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Address { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public string Username { get; set; }

        // details additions
        public ICollection<AdminPaneOrderProductModel> Products { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal TotalPrice { get; set; }

        public int Quantity { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public PaymentType PaymentType { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, AdminPaneOrderViewModel>()
                .ForMember(model => model.Address, opt => opt.MapFrom(src => src.DeliveryAddress.AddressText))
                .ForMember(model => model.Username, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(model => model.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(model => model.Products, opt => opt.MapFrom(src => src.OrderProducts));

            configuration.CreateMap<OrderProduct, AdminPaneOrderProductModel>()
                .ForMember(model => model.ImageName, opt => opt.MapFrom(src => src.Product.ImageName))
                .ForMember(model => model.Name, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}
