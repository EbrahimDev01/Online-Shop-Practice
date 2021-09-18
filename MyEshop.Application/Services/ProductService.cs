using MyEshop.Application.Interfaces;
using MyEshop.Application.Utilities.File;
using MyEshop.Application.ViewModels.Product;
using MyEshop.Application.ViewModels.Public;
using MyEshop.Domain.ConstsDomain.Messages;
using MyEshop.Domain.Interfaces;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        private readonly ITagRepository _tagRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ICommentRepository _commentRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository,
            ITagRepository tagRepository, IImageRepository imageRepository,
            ICommentRepository commentRepository)
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _imageRepository = imageRepository;
            _categoryService = new CategoryService(categoryRepository);
            _commentRepository = commentRepository;
        }

        public async ValueTask<ResultMethodService> CreateProductAsync(ProductCreateViewModel createProduct)
        {
            var resultMethodService = new ResultMethodService();

            bool isExistCategory = await _categoryRepository.IsExistCategoryAsync(createProduct.CategoryId);

            var tagIdesSelected = createProduct.Tags.Where(t => t.IsSelected).Select(t => t.Id);
            var tags = _tagRepository.GetTagsByIds(tagIdesSelected).ToList();

            if (!isExistCategory)
                resultMethodService.AddError(nameof(createProduct.CategoryId), ErrorMessage.NotExistCategory);

            if (tagIdesSelected?.Count() > 0 && tags?.Count <= 0)
                resultMethodService.AddError(nameof(createProduct.Tags), ErrorMessage.NotExistTag);

            if (!resultMethodService.IsSuccess)
                return resultMethodService;

            var product = new Product
            {
                CategoryId = createProduct.CategoryId,
                CreateDateTime = DateTime.Now,
                Descritption = createProduct.Descritption,
                Explanation = createProduct.Explanation,
                Price = createProduct.Price,
                QuantityInStok = createProduct.QuantityInStok,
                Title = createProduct.Title,
                Views = 0,
                Tags = tags
            };

            if (createProduct?.Images?.Count > 0 && createProduct?.Images?.FirstOrDefault().Length > 0)
                foreach (var imageItem in createProduct.Images)
                {
                    string resultNameFile = await CreateFile.CreateAsync(imageItem);

                    if (resultNameFile == null)
                    {
                        resultMethodService.AddError(nameof(Product.Images), ErrorMessage.NotSave);

                        return resultMethodService;
                    }

                    product.Images.Add(new Image(resultNameFile));
                }

            bool isCreate = await _productRepository.CreateProductAsync(product);
            bool isSave = await _productRepository.SaveAsync();

            resultMethodService.IsSuccess = isCreate && isSave;

            if (!isCreate && !isSave)
                resultMethodService.AddError("", ErrorMessage.NotSave);

            return resultMethodService;
        }

        public IAsyncEnumerable<PreviewAdminProductViewModel> GetAllPreviewAdminProductsAsync()
            => _productRepository.GetProductsAllAsync()?
            .SelectAwait(async p => new PreviewAdminProductViewModel(p,
                await _imageRepository.GetFirstImageUrlProductByProductIdAsync(p.ProductId)));

        public async ValueTask<ProductDeleteViewModel> GetProductDeleteViewByProductIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null) return null;

            var category = await _categoryService.GetCategorieChildrenByIdAsync(product.CategoryId);
            var tags = _tagRepository.GetTagsProductByProductId(productId);
            var images = _imageRepository.GetImagesProductByProductId(productId).Select(image => image.UrlImage);
            int commentCount = _commentRepository.GetCommentCountProductByProductId(productId);

            return new(product, category, tags, images, commentCount);
        }
    }
}

