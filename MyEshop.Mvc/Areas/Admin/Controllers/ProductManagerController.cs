using Microsoft.AspNetCore.Mvc;
using MyEshop.Application.Interfaces;
using MyEshop.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEshop.Mvc.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class ProductManagerController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public ProductManagerController(IProductService productService, ITagService tagService
            , ICategoryService categoryService)
        {
            _productService = productService;
            _tagService = tagService;
            _categoryService = categoryService;

        }

        public IActionResult Index()
        {
            var products = _productService.GetAllPreviewAdminProductsAsync();

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var tags = await _tagService.GetTagsForSelectAsync();
            var categories = _categoryService.GetCategoriesChildrenAsync();

            var createProductModel = new ProductCreateViewModel(categories, tags);

            return View(createProductModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel createProduct)
        {
            if (!ModelState.IsValid)
                return View(createProduct);

            var resultCreateProduct = await _productService.CreateProductAsync(createProduct);

            if (resultCreateProduct.IsSuccess)
                return RedirectToAction(nameof(Index));

            foreach (var error in resultCreateProduct.Errors)
                ModelState.AddModelError(error.Title, error.Message);

            createProduct.Tags = await _tagService.GetTagsForSelectAsync();
            createProduct.Categories = _categoryService.GetCategoriesChildrenAsync();


            return View(createProduct);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductDeleteViewByProductIdAsync(id);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var resultProductDelete = await _productService.DeleteAsync(id);

            if (resultProductDelete.IsNotFound)
                return NotFound();

            if (resultProductDelete.IsSuccess)
                return RedirectToAction(nameof(Index));

            foreach (var error in resultProductDelete)
                ModelState.AddModelError(error.Title, error.Message);

            var product = await _productService.GetProductDeleteViewByProductIdAsync(id);

            return View(product);
        }
    }
}
