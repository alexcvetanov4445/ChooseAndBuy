namespace ChooseAndBuy.Web.ViewModels.Orders
{
    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class OrderProductViewModel : IMapFrom<ShoppingCartProduct>, IMapTo<OrderProduct>, IHaveCustomMappings
    {
        public string ProductId { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }

        public string ImageName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ShoppingCartProduct, OrderProductViewModel>()
                .ForMember(model => model.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(model => model.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(model => model.ImageName, opt => opt.MapFrom(src => src.Product.ImageName))
                .ForMember(model => model.TotalPrice, opt => opt.MapFrom(src => (double)(src.Quantity * src.Product.Price)));
        }
    }
}
