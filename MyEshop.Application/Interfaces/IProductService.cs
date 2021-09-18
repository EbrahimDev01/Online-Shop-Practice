using MyEshop.Application.ViewModels.Product;
using MyEshop.Application.ViewModels.Public;
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

        public ValueTask<ProductDeleteViewModel> GetProductDeleteViewByProductIdAsync(int productId);
    }
}
