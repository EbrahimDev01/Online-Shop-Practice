using Microsoft.AspNetCore.Http;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.Utilities.File
{
    public class FileHandler : IFileHandler
    {
        public async ValueTask<string> CreateAsync(IFormFile formFile, string newPath = null)
        {
            if (formFile == null)
                return null;

            try
            {
                string fileExtension = Path.GetExtension(formFile.FileName);
                string newFileName = Guid.NewGuid() + fileExtension;
                string newPathFile = "";

                if (string.IsNullOrEmpty(newPath))
                {
                    newPathFile = Path.Combine(Directory.GetCurrentDirectory(),
                                               "wwwroot",
                                               "image",
                                               newFileName);
                }
                else
                {
                    newPathFile = Path.Combine(newPath,
                                               "wwwroot",
                                               "image",
                                               newFileName);
                }

                using var fileStream = System.IO.File.Create(newPathFile);

                await formFile.CopyToAsync(fileStream);

                newPath = Path.Combine("/image/", newFileName);

                return newPath;
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(IEnumerable<Image> images)
        {
            try
            {
                foreach (var image in images)
                {
                    string urlImage = image.UrlImage.Remove(0, 7);

                    string pathFile = Path.Combine(Directory.GetCurrentDirectory(),
                                                       "wwwroot", "image", urlImage);

                    System.IO.File.Delete(pathFile);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
