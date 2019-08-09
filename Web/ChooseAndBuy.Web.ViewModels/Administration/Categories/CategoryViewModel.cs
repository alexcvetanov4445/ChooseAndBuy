namespace ChooseAndBuy.Web.ViewModels.Administration.Categories
{
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
