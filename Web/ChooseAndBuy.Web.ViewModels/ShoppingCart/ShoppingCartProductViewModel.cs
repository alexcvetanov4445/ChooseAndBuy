namespace ChooseAndBuy.Web.ViewModels.ShoppingCart
{
    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;
    using ChooseAndBuy.Web.ViewModels.Orders;

    public class ShoppingCartProductViewModel : IMapFrom<OrderProductViewModel>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string ImageName { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public double TotalPrice { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderProductViewModel, ShoppingCartProductViewModel>()
                .ForMember(model => model.Id, opt => opt.MapFrom(src => src.ProductId));
        }
    }
}
