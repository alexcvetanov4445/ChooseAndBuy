namespace ChooseAndBuy.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<string> CreateImage(IFormFile formImage, string fileName);
    }
}
