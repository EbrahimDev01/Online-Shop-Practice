using Microsoft.AspNetCore.Http;
using MyEshop.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MyEshop.Application.Utilities.File
{
    public interface IFileHandler
    {
        public readonly static string ImageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image");

        public ValueTask<string> CreateAsync(IFormFile formFile, string pathFile = null);


        public bool DeleteImages(IEnumerable<string> namesFile, string pathFile = null);

        public string CombinePahtAndNameFile(string fileName, string pathFile = null);

        public bool IsImage(IEnumerable<IFormFile> formFiles);
    }
}
