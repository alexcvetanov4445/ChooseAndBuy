namespace ChooseAndBuy.Services
{
    using System;
    using System.IO;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    public class ImageService : IImageService
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public ImageService(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public string CreateImage(IFormFile formImage)
        {
            string uniqueFileName = string.Empty;

            if (formImage != null)
            {
                string uploadFolder = Path.Combine(this.hostingEnvironment.WebRootPath, "img", "product");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + formImage.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                // creating the file to wwwroot/img folder
                formImage.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return uniqueFileName;
        }
    }
}
