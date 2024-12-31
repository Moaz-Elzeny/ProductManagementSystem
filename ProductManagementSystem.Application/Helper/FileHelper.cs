using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace ProductManagementSystem.Application.Helper
{
    public class FileHelper
    {
        public static async Task<string> SaveImageAsync(IFormFile image, IHostingEnvironment _environment)
        {
            if (!Directory.Exists(_environment.WebRootPath + "/uploads/images/"))
            {
                Directory.CreateDirectory(_environment.WebRootPath + "/uploads/images/");
            }
            string ImagePath = "/uploads/images/" + GenerateUniqueID(25) + Path.GetExtension(image.FileName).ToLower();
            using (FileStream fileStream = File.Create(_environment.WebRootPath + ImagePath))
            {
                image.CopyTo(fileStream);
                fileStream.Flush();
                var fileExtention = Path.GetExtension(image.FileName).ToLower();
            }
            return ImagePath;
        }

        public static string GenerateUniqueID(int _characterLength = 15)
        {
            StringBuilder _builder = new StringBuilder();
            Enumerable
                .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(_characterLength)
                .ToList().ForEach(e => _builder.Append(e));
            return _builder.ToString();
        }
    }
}
