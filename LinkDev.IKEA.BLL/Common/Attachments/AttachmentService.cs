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

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {

            var extension = Path.GetExtension(file.FileName);

            if (!_allowedExtwnsions.Contains(extension))
                throw new Exception("Invalid File Extension");

            if (file.Length > _allowedMaxSize)
                throw new Exception("Invalid File Size");


            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files",folderName);


            if (!Directory.Exists(folderPath)) 
                Directory.CreateDirectory(folderPath);


            var fileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(folderPath, fileName);

            // Streaming

            //using var fileStream = new FileStream(filePath, FileMode.Create);

            using var fileStream = File.Create(filePath);

            await file.CopyToAsync(fileStream);

            return  fileName;





        }


        public bool DeleteFile(string filePath)
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
