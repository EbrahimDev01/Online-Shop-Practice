using MyEshop.Application.Interfaces;
using MyEshop.Application.Utilities.File;
using MyEshop.Application.ViewModels.Image;
using MyEshop.Application.ViewModels.Product;
using MyEshop.Application.ViewModels.PublicViewModelClass;
using MyEshop.Application.ViewModels.Tag;
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
                resultMethodService.AddError(nameof(createProduct.CategoryId), ErrorMessage.ExceptionExistCategory);

            if (tagIdesSelected?.Count() > 0 && tags?.Count <= 0)
                resultMethodService.AddError(nameof(createProduct.Tags), ErrorMessage.ExceptionExistTags);

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
                    string resultNameFile = await FileCreate.CreateAsync(imageItem);

                    if (resultNameFile == null)
                    {
                        resultMethodService.AddError(nameof(Product.Images), ErrorMessage.ExceptionSave);

                        return resultMethodService;
                    }

                    product.Images.Add(new Image(resultNameFile));
                }

            bool isCreate = await _productRepository.CreateProductAsync(product);
            bool isSave = await _productRepository.SaveAsync();

            resultMethodService.IsSuccess = isCreate && isSave;

            if (!isCreate && !isSave)
                resultMethodService.AddError("", ErrorMessage.ExceptionSave);

            return resultMethodService;
        }

        public async ValueTask<ResultMethodService> DeleteProductByProductIdAsync(int productId)
        {
            var resultMethod = new ResultMethodService();

            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product == null)
            {
                resultMethod.NotFound();
                return ReturnMethodWithErrorMessage(ErrorMessage.NotFound("محصول"));
            }

            bool isDeleteComments = _commentRepository.DeleteCommentsByProductId(productId);

            if (!isDeleteComments)
                return ReturnMethodWithErrorMessage(ErrorMessage.ExceptionCommentsDelete);

            IEnumerable<Image> imagesProduct;

            try
            {
                imagesProduct = _imageRepository.GetImagesProductByProductId(productId);
            }
            catch
            {
                return ReturnMethodWithErrorMessage(ErrorMessage.ExceptionImagesFind);
            }

            var imagesProductCopy = new List<Image>(imagesProduct);

            bool isDeleteImages = await _imageRepository.DeleteImagesAsync(imagesProduct);

            if (imagesProduct != null && !isDeleteImages)
                return ReturnMethodWithErrorMessage(ErrorMessage.ExceptionImagesDelete);


            bool isDeleteProduct = await _productRepository.DeleteProductAsync(product);

            if (!isDeleteProduct)
                return ReturnMethodWithErrorMessage(ErrorMessage.ExceptionProductDelete);

            bool isSave = await _productRepository.SaveAsync();

            if (!isSave)
                return ReturnMethodWithErrorMessage(ErrorMessage.ExceptionSave);

            if (imagesProductCopy?.Count > 0)
            {
                bool isDeletFiles = FileDelete.Delete(imagesProductCopy);

                if (!isDeletFiles)
                    return ReturnMethodWithErrorMessage(ErrorMessage.ExceptionFileImagesDelete);
            }

            return resultMethod;

            ResultMethodService ReturnMethodWithErrorMessage(string errorMessage)
            {
                resultMethod.AddError(string.Empty, errorMessage);

                return resultMethod;
            }
        }

        public IAsyncEnumerable<PreviewAdminProductViewModel> GetAllPreviewAdminProductsAsync()
            => _productRepository.GetProductsAllAsync()?
            .SelectAwait(async p => new PreviewAdminProductViewModel(p,
                await _imageRepository.GetFirstImageUrlProductByProductIdAsync(p.ProductId)));

        public async ValueTask<ProductDetailsViewModel> GetProductDetailsByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null) return null;

            var category = await _categoryService.GetCategorieChildrenByIdAsync(product.CategoryId);
            var tags = _tagRepository.GetTagsProductByProductId(productId);
            var images = _imageRepository.GetImagesProductByProductId(productId).Select(image => image.UrlImage);
            int commentCount = _commentRepository.GetCommentCountProductByProductId(productId);

            return new(product, category, tags, images, commentCount);
        }

        public async ValueTask<ProductEditViewModel> GetProductEditDetailsByProductIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product is null)
                return null;

            var categories = _categoryService.GetCategoriesChildrenAsync();

            var tagsProduct = _tagRepository.GetTagsProductByProductId(productId);
            
            var tags = await _tagRepository.GetTagsAsync()
                .Select(tag => new TagForSelect(tag)
                {
                    IsSelected = tagsProduct.Any(tagProduct => tag.TagId == tagProduct.TagId)
                })
                .ToListAsync();

            var images = _imageRepository.GetImagesProductByProductId(productId)
                .Select(image => new SelectImageToDelete(image))
                .ToList();

            return new ProductEditViewModel(product, categories, tags, images);
        }
    }
}