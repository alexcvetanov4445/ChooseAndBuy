namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Products;
    using ChooseAndBuy.Web.ViewModels.Products;

    public interface IProductService
    {
        Task<bool> ProductExists(string name);

        Task<bool> AddProduct(CreateProductBindingModel model);

        Task<bool> HideProduct(string id);

        Task<bool> RecommendProduct(string íd);

        Task<bool> EditProduct(EditProductBindingModel model);

        Task<EditProductBindingModel> GetEditProductInfoById(string productId);

        Task<string> GetIdByName(string productName);

        Task<IEnumerable<ProductViewModel>> GetProducts(string search, string subCategoryId, int sortBy);

        Task<IEnumerable<TableProductViewModel>> GetAllProducts();

        Task<ProductDetailsViewModel> GetById(string id);

        Task<IEnumerable<RecommendedProductViewModel>> GetRecommendationProducts();
    }
}
