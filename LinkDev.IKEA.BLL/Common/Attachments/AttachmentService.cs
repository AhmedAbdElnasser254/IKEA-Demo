using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Common.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        private readonly List<string> _allowedExtwnsions = new() { ".png", ".jpg", ".jpeg" };
        private const int _allowedMaxSize = 2_097_152;

        public string? Upload(IFormFile file, string folderName)
        {

            var extension = Path.GetExtension(file.FileName);

            if (!_allowedExtwnsions.Contains(extension))
                return null;

            if (file.Length > _allowedMaxSize)
                return null;


            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files",folderName);


            if (!Directory.Exists(folderPath)) 
                Directory.CreateDirectory(folderPath);


            var fileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(folderPath, fileName);

            // Streaming

            //using var fileStream = new FileStream(filePath, FileMode.Create);

            using var fileStream = File.Create(filePath);

            file.CopyTo(fileStream);
            return fileName;





        }


        public bool Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

    }
}
