using Microsoft.AspNetCore.Http;
using MyEshop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyEshop.Application.Utilities.File
{
    public interface IFileHandler
    {
        public ValueTask<string> CreateAsync(IFormFile formFile, string newPath = null);
        public bool Delete(IEnumerable<Image> images);
    }
}
