namespace ChooseAndBuy.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class ProductDetailsViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string ImageName { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public ICollection<ProductReviewViewModel> Reviews { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(model => model.Category, opt => opt.MapFrom(src => src.SubCategory.Name));
        }
    }
}
