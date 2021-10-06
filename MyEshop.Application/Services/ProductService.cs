using MyEshop.Application.ConstApplication.Names;
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
        private readonly IFileHandler _fileHandler;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository,
            ITagRepository tagRepository, IImageRepository imageRepository,
            ICommentRepository commentRepository, IFileHandler fileHandler)
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _imageRepository = imageRepository;
            _categoryService = new CategoryService(categoryRepository);
            _commentRepository = commentRepository;
            _fileHandler = fileHandler;
        }

        public async ValueTask<ResultMethodService> CreateProductAsync(ProductCreateViewModel createProductModel)
        {
            var resultMethodService = new ResultMethodService();

            bool isExistCategory = await _categoryRepository.IsExistCategoryAsync(createProductModel.CategoryId);

            var tagIdesSelected = createProductModel.Tags.Where(t => t.IsSelected).Select(t => t.Id);
            var tags = _tagRepository.GetTagsByIds(tagIdesSelected).ToList();


            bool fileTypeIsImage = _fileHandler.IsImage(createProductModel.Images);

            if (!isExistCategory)
            {
                resultMethodService.AddError(nameof(createProductModel.CategoryId), ErrorMessage.ExceptionExistCategory);
            }

            if (tagIdesSelected?.Count() > 0 && tags?.Count <= 0)
            {
                resultMethodService.AddError(nameof(createProductModel.Tags), ErrorMessage.ExceptionExistTags);
            }

            if (!fileTypeIsImage)
            {
                resultMethodService.AddError(nameof(ProductEditViewModel.Images), ErrorMessage.ExceptionFileImagesType);
            }

            if (!resultMethodService.IsSuccess)
                return resultMethodService;

            var product = new Product
            {
                CategoryId = createProductModel.CategoryId,
                CreateDateTime = DateTime.Now,
                Descritption = createProductModel.Descritption,
                Explanation = createProductModel.Explanation,
                Price = createProductModel.Price,
                QuantityInStok = createProductModel.QuantityInStok,
                Title = createProductModel.Title,
                Views = 0,
                Tags = tags
            };

            if (createProductModel?.Images?.Count > 0 && createProductModel?.Images?.FirstOrDefault().Length > 0)
            {
                foreach (var imageItem in createProductModel.Images)
                {
                    string resultNameFile = await _fileHandler.CreateAsync(imageItem);

                    if (resultNameFile == null)
                    {
                        resultMethodService.AddError(nameof(Product.Images), ErrorMessage.ExceptionSave);

                        return resultMethodService;
                    }

                    product.Images.Add(new Image(resultNameFile));
                }
            }

            bool isCreate = await _productRepository.CreateProductAsync(product);
            if (!isCreate)
            {
                resultMethodService.AddError(string.Empty, ErrorMessage.ExceptionProductCreate("محصول"));

                return resultMethodService;
            }

            bool isSave = await _productRepository.SaveAsync();
            if (!isSave)
            {
                resultMethodService.AddError(string.Empty, ErrorMessage.ExceptionSave);

                return resultMethodService;
            }

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
            {
                return ReturnMethodWithErrorMessage(ErrorMessage.ExceptionCommentsDelete);
            }


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
            {
                return ReturnMethodWithErrorMessage(ErrorMessage.ExceptionImagesDeletse);
            }

            bool isDeleteProduct = await _productRepository.DeleteProductAsync(product);

            if (!isDeleteProduct)
            {
                return ReturnMethodWithErrorMessage(ErrorMessage.ExceptionProductDelete);
            }

            bool isSave = await _productRepository.SaveAsync();

            if (!isSave)
            {
                return ReturnMethodWithErrorMessage(ErrorMessage.ExceptionSave);
            }

            if (imagesProductCopy?.Count > 0)
            {
                bool isDeletFiles = _fileHandler.DeleteImages(imagesProductCopy);

                if (!isDeletFiles)
                {
                    return ReturnMethodWithErrorMessage(ErrorMessage.ExceptionFileImagesDelete);
                }
            }

            return resultMethod;

            ResultMethodService ReturnMethodWithErrorMessage(string errorMessage)
            {
                resultMethod.AddError(string.Empty, errorMessage);

                return resultMethod;
            }
        }

        public async ValueTask<ResultMethodService> EditProductAsync(ProductEditViewModel editProductModel)
        {
            var resultMethodService = new ResultMethodService();

            var product = await _productRepository.GetProductByIdAsync(editProductModel.ProductId);
            if (product is null)
            {
                resultMethodService.NotFound();

                return resultMethodService;
            }

            bool isExistCategory = await _categoryRepository.IsExistCategoryAsync(product.CategoryId);

            var tagIdesSelected = editProductModel.Tags.Where(t => t.IsSelected).Select(t => t.Id);
            var tags = _tagRepository.GetTagsByIds(tagIdesSelected).ToList();

            bool fileTypeIsImage = _fileHandler.IsImage(editProductModel.Images);

            var availableImagesTypeClassImage = editProductModel.AvailableImages
                .Where(image => image.IsSelected)
                .Select(image => new Image(image.ImageId, image.UrlImage));

            bool isExistAvailableImages = _imageRepository.IsExistAvailableImages(availableImagesTypeClassImage, editProductModel.ProductId);

            var isDeleteTags = await _productRepository.DeleteTagsProductByProductIdAsync(product.ProductId);


            if (!isExistCategory)
            {
                resultMethodService.AddError(nameof(ProductEditViewModel.CategoryId), ErrorMessage.ExceptionExistCategory);
            }

            if (tagIdesSelected?.Count() > 0 && tags?.Count <= 0)
            {
                resultMethodService.AddError(nameof(ProductEditViewModel.Tags), ErrorMessage.ExceptionExistTags);
            }

            if (!fileTypeIsImage)
            {
                resultMethodService.AddError(nameof(ProductEditViewModel.Images), ErrorMessage.ExceptionFileImagesType);
            }

            if (availableImagesTypeClassImage.Any() && !isExistAvailableImages)
            {
                resultMethodService.AddError(nameof(ProductEditViewModel.Images), ErrorMessage.ExceptionAvailableImages);
            }

            if (!isDeleteTags)
            {
                resultMethodService.AddError(nameof(ProductEditViewModel.Tags), ErrorMessage.ExceptionTagsDelete);
            }

            if (!resultMethodService.IsSuccess)
            {
                return resultMethodService;
            }


            product.CategoryId = editProductModel.CategoryId;
            product.Title = editProductModel.Title;
            product.Price = editProductModel.Price;
            product.QuantityInStok = editProductModel.QuantityInStok;
            product.Explanation = editProductModel.Explanation;
            product.Descritption = editProductModel.Descritption;
            product.Tags = tags;

            if (editProductModel?.Images?.Count > 0 && editProductModel?.Images?.FirstOrDefault().Length > 0)
            {
                foreach (var imageItem in editProductModel.Images)
                {
                    string resultNameFile = await _fileHandler.CreateAsync(imageItem);

                    if (resultNameFile == null)
                    {
                        _fileHandler.DeleteImages(product.Images);

                        resultMethodService.AddError(nameof(Product.Images), ErrorMessage.ExceptionFileSave);

                        return resultMethodService;
                    }

                    product.Images.Add(new Image(resultNameFile));
                }
            }

            if (isExistAvailableImages && availableImagesTypeClassImage.Any())
            {
                await _imageRepository.DeleteImagesByImageIdsAsync(availableImagesTypeClassImage.Select(image => image.ImageId));
            }

            bool isEdit = await _productRepository.EditProductAsync(product);
            if (!isEdit)
            {
                resultMethodService.AddError(string.Empty, ErrorMessage.ExceptionProductEdit("محصول"));

                return resultMethodService;
            }

            bool isSave = await _productRepository.SaveAsync();
            if (!isSave)
            {
                resultMethodService.AddError(string.Empty, ErrorMessage.ExceptionSave);

                return resultMethodService;
            }

            if (isExistAvailableImages && availableImagesTypeClassImage.Any())
            {
                bool isDeleteImages = _fileHandler.DeleteImages(availableImagesTypeClassImage);

                if (!isDeleteImages)
                    resultMethodService.AddError(nameof(ProductEditViewModel.Images), ErrorMessage.ExceptionFileImagesDelete);
            }

            return resultMethodService;
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