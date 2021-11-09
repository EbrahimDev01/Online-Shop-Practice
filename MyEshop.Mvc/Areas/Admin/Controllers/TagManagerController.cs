using Microsoft.AspNetCore.Mvc;
using MyEshop.Application.Interfaces;
using MyEshop.Application.ViewModels.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEshop.Mvc.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class TagManagerController : Controller
    {
        private readonly ITagService _tagService;

        public TagManagerController(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IActionResult Index()
        {
            var tags = _tagService.GetAllTagsAsync();
            return View(tags);
        }

        public async Task<IActionResult> Details(int id)
        {

            var tag = await _tagService.GetTagDetailsByTagIdAsync(id);

            if (tag is null)
            {
                return NotFound();
            }

            return View(tag);
        }

        #region Creatae

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagCreateViewModel tagCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(tagCreateModel);
            }

            var tagCreateResult = await _tagService.CreateTagAsync(tagCreateModel);

            if (tagCreateResult.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in tagCreateResult.Errors)
            {
                ModelState.AddModelError(error.Title, error.Message);
            }

            return View(tagCreateModel);
        }


        #endregion

        #region Edit

        public async Task<IActionResult> Edit(int id)
        {
            var resultTag = await _tagService.GetTagShapeEditViewModelByTagIdAsync(id);

            if (resultTag is null)
            {
                return NotFound();
            }

            return View(resultTag);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TagEditViewModel tagEditModel)
        {
            if (!ModelState.IsValid)
            {
                return View(tagEditModel);
            }

            var resultEditTag = await _tagService.EditTagAsync(tagEditModel);

            if (resultEditTag.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            if (resultEditTag.IsNotFound)
            {
                return NotFound();
            }

            foreach (var error in resultEditTag.Errors)
            {
                ModelState.AddModelError(error.Title, error.Message);
            }

            return View(tagEditModel);
        }


        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _tagService.GetTagShapeDeleteViewModelByTagIdAsync(id);

            if (tag is null)
            {
                return NotFound();
            }

            return View(tag);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var resultTagDelete = await _tagService.DeleteTagAsync(id);


            if (resultTagDelete.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            if (resultTagDelete.IsNotFound)
            {
                return NotFound();
            }

            foreach (var error in resultTagDelete.Errors)
            {
                ModelState.AddModelError(error.Title, error.Message);
            }

            var tag = _tagService.GetTagShapeDeleteViewModelByTagIdAsync(id);

            return View(tag);
        }

        #endregion

        [HttpPost]
        public async Task<JsonResult> IsExistTagByTitle(string title)
        {
            bool isExistTag = !await _tagService.IsExistTagByTitle(title);

            return Json(isExistTag);
        }

        [HttpPost]
        public async Task<JsonResult> IsExistTagByTagTitleAndTagId(string title, int tagId)
        {
            bool isExistTag = !await _tagService.IsExistTagByTagTitleAndTagId(title, tagId);

            return Json(isExistTag);
        }
    }
}
