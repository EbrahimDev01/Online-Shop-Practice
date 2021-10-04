using Microsoft.EntityFrameworkCore;
using MyEshop.Data.Context;
using MyEshop.Domain.Interfaces;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly MyEshopDBContext _dbContext;

        public ImageRepository(MyEshopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ValueTask<bool> DeleteImagesAsync(IEnumerable<Image> images)
        {
            try
            {
                _dbContext.Images.RemoveRange(images);

                return ValueTask.FromResult(true);
            }
            catch
            {
                return ValueTask.FromResult(false);
            }
        }

        public async ValueTask<string> GetFirstImageUrlProductByProductIdAsync(int productId)
            => (await _dbContext.Images.FirstOrDefaultAsync(image => image.ProductId == productId)).UrlImage;

        public IEnumerable<Image> GetImagesByImageIds(IEnumerable<int> imageIds)
        {
            foreach (var imageId in imageIds)
                yield return _dbContext.Images.Find(imageId);
        }

        public IEnumerable<Image> GetImagesProductByProductId(int productId) =>
            _dbContext.Images.Where(image => image.ProductId == productId);

        public bool IsExistAvailableImages(IEnumerable<Image> images, int productId)
            => images.All(image => _dbContext.Images.Any(imagedb =>
                     imagedb.ProductId == productId && imagedb.ImageId == image.ImageId && imagedb.UrlImage == image.UrlImage));
        public async ValueTask<bool> DeleteImagesByImageIdsAsync(IEnumerable<int> imageIds)
        {
            try
            {
                var images = GetImagesByImageIds(imageIds);


                return await DeleteImagesAsync(images);
            }
            catch
            {
                return false;
            }
        }

    }
}
