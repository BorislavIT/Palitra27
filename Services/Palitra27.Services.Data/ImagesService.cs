namespace Palitra27.Services.Data
{
    using System.IO;

    using Microsoft.AspNetCore.Http;

    public class ImagesService : IImagesService
    {
        public async void UploadImage(IFormFile formImage, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formImage.CopyToAsync(stream);
            }
        }
    }
}