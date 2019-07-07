namespace ChooseAndBuy.Web.MappingConfiguration
{
    using System.Collections.Generic;

    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Categories;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Products;
    using ChooseAndBuy.Web.ViewModels.Products;
    using ChooseAndBuy.Web.ViewModels.ShoppingCart;

    public class ChooseAndBuyProfile : Profile
    {
        public ChooseAndBuyProfile()
        {
            this.CreateMap<CreateProductBindingModel, Product>();
            this.CreateMap<CreateCategoryBindingModel, Category>();
            this.CreateMap<Product, ProductViewModel>();
            this.CreateMap<Review, ProductReviewViewModel>().ReverseMap();
            this.CreateMap<ReviewBindingModel, Review>();


            this.CreateMap<ShoppingCartProduct, ShoppingCartProductViewModel>()
                .ForMember(model => model.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(model => model.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(model => model.ImageName, opt => opt.MapFrom(src => src.Product.ImageName))
                .ForMember(model => model.TotalPrice, opt => opt.MapFrom(src => (double)(src.Quantity * src.Product.Price)));

            this.CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(model => model.Category, opt => opt.MapFrom(src => src.SubCategory.Name));
        }
    }
}
