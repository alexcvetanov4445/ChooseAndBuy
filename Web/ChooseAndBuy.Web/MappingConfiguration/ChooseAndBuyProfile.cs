namespace ChooseAndBuy.Web.MappingConfiguration
{
    using System.Collections.Generic;

    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Categories;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Orders;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Products;
    using ChooseAndBuy.Web.ViewModels.Addresses;
    using ChooseAndBuy.Web.ViewModels.Orders;
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
            this.CreateMap<Product, EditProductBindingModel>().ReverseMap();
            this.CreateMap<AddressCreateBindingModel, Address>();
            this.CreateMap<Address, AddressViewModel>();
            this.CreateMap<OrderBindingModel, Order>();
            this.CreateMap<OrderProductViewModel, OrderProduct>();

            this.CreateMap<OrderProduct, AdminPaneOrderProductModel>()
                .ForMember(model => model.ImageName, opt => opt.MapFrom(src => src.Product.ImageName))
                .ForMember(model => model.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(model => model.Quantity, opt => opt.MapFrom(src => src.Quantity));

            this.CreateMap<Order, AdminPaneOrderViewModel>()
                .ForMember(model => model.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(model => model.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(model => model.Address, opt => opt.MapFrom(src => src.DeliveryAddress.AddressText))
                .ForMember(model => model.Username, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(model => model.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(model => model.Products, opt => opt.MapFrom(src => src.OrderProducts));

            this.CreateMap<Order, OrderViewModel>()
                .ForMember(model => model.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(model => model.ExpectedDelivery, opt => opt.MapFrom(src => src.DispatchDate.Value.ToShortDateString()))
                .ForMember(model => model.Address, opt => opt.MapFrom(src => src.DeliveryAddress.AddressText))
                .ForMember(model => model.Price, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(model => model.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(model => model.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            this.CreateMap<ShoppingCartProduct, OrderProductViewModel>()
                .ForMember(model => model.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(model => model.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(model => model.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(model => model.TotalPrice, opt => opt.MapFrom(src => (double)(src.Quantity * src.Product.Price)));

            this.CreateMap<ShoppingCartProduct, ShoppingCartProductViewModel>()
                .ForMember(model => model.Id, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(model => model.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(model => model.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(model => model.ImageName, opt => opt.MapFrom(src => src.Product.ImageName))
                .ForMember(model => model.TotalPrice, opt => opt.MapFrom(src => (double)(src.Quantity * src.Product.Price)));

            this.CreateMap<Product, TableProductViewModel>()
                .ForMember(model => model.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name));

            this.CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(model => model.Category, opt => opt.MapFrom(src => src.SubCategory.Name));
        }
    }
}
