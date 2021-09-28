using MyEshop.Application.ViewModels.Product;
using MyEshop.Application.ViewModels.PublicViewModelClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.Interfaces
{
    public interface IProductService
    {
        public IAsyncEnumerable<PreviewAdminProductViewModel> GetAllPreviewAdminProductsAsync();

        public ValueTask<ResultMethodService> CreateProductAsync(ProductCreateViewModel createProduct);

        public ValueTask<ProductDetailsViewModel> GetProductDetailsByIdAsync(int productId);

        public ValueTask<ResultMethodService> DeleteProductByProductIdAsync(int productId);

        public ValueTask<ProductEditViewModel> GetProductEditDetailsByProductIdAsync(int productId);
    }
}
