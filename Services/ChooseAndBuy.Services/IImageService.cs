namespace ChooseAndBuy.Services
{
    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        string CreateImage(IFormFile formImage);
    }
}
