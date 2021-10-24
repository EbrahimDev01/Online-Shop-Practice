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

        [HttpPost]
        public async Task<JsonResult> IsExistTagByTitle(string title)
        {
            bool isExistTag = !await _tagService.IsExistTagByTitle(title);

            return Json(isExistTag);
        }
    }
}
