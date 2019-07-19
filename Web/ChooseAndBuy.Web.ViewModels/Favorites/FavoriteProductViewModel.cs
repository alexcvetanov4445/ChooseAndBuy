namespace ChooseAndBuy.Web.ViewModels.Favorites
{
    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class FavoriteProductViewModel : IMapFrom<UserFavoriteProduct>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImageName { get; set; }

        public decimal Price { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserFavoriteProduct, FavoriteProductViewModel>()
                .ForMember(model => model.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(model => model.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(model => model.ImageName, opt => opt.MapFrom(src => src.Product.ImageName))
                .ForMember(model => model.Price, opt => opt.MapFrom(src => src.Product.Price));
        }
    }
}
