namespace ChooseAndBuy.Web.ViewModels.Administration.Products
{
    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class TableProductViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsRecommended { get; set; }

        public bool IsHidden { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public decimal Price { get; set; }

        public string SubCategoryName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, TableProductViewModel>()
                .ForMember(model => model.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name));
        }
    }
}
