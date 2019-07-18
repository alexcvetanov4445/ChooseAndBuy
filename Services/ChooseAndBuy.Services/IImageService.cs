namespace ChooseAndBuy.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        Task<string> CreateImage(IFormFile formImage);
    }
}
