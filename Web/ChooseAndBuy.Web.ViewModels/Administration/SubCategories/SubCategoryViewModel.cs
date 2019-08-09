namespace ChooseAndBuy.Web.ViewModels.Administration.SubCategories
{
    using AutoMapper;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class SubCategoryViewModel : IMapFrom<SubCategory>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string MainCategory { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<SubCategory, SubCategoryViewModel>()
                .ForMember(model => model.MainCategory, opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}
