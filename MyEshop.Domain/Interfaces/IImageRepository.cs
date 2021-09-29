using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Interfaces
{
    public interface IImageRepository
    {
        public IEnumerable<Image> GetImagesProductByProductId(int productId);
        public ValueTask<string> GetFirstImageUrlProductByProductIdAsync(int productId);
        public ValueTask<bool> DeleteImagesAsync(IEnumerable<Image> images);
        public bool IsExistAvailableImages(IEnumerable<Image> images, int productId);
    }
}
