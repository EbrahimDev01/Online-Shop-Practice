using Microsoft.AspNetCore.Mvc;
using MyEshop.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEshop.Mvc.Areas.Admin.Controllers
{
    public class TagManagerController : Controller
    {
        private readonly ITagService _tagService;

        public TagManagerController(ITagService tagService)
        {
            _tagService = tagService;
        }
    }
}
