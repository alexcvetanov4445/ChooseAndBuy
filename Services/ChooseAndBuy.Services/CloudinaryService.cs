namespace ChooseAndBuy.Services
{
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly Cloudinary cloudinaryUtility;

        public CloudinaryService(
            IHostingEnvironment hostingEnvironment,
            Cloudinary cloudinaryUtility)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.cloudinaryUtility = cloudinaryUtility;
        }

        public async Task<string> CreateImage(IFormFile formImage, string fileName)
        {
            byte[] destinationData;

            using (var ms = new MemoryStream())
            {
                await formImage.CopyToAsync(ms);
                destinationData = ms.ToArray();
            }

            UploadResult uploadResult = null;

            using (var ms = new MemoryStream(destinationData))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = "products_img",
                    File = new FileDescription(fileName, ms),
                };

                uploadResult = this.cloudinaryUtility.Upload(uploadParams);
            }

            return uploadResult?.SecureUri.AbsoluteUri;
        }
    }
}
