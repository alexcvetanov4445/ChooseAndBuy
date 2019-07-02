namespace ChooseAndBuy.Web.MappingConfiguration
{
    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Categories;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Products;
    using ChooseAndBuy.Web.ViewModels.Products;

    public class ChooseAndBuyProfile : Profile
    {
        public ChooseAndBuyProfile()
        {
            this.CreateMap<CreateProductBindingModel, Product>();
            this.CreateMap<CreateCategoryBindingModel, Category>();
            this.CreateMap<Product, ProductViewModel>();
            this.CreateMap<Review, ProductReviewViewModel>();

            this.CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(cat => cat.Category, opt => opt.MapFrom(src => src.SubCategory.Name));
        }
    }
}
