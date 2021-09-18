using MyEshop.Application.ConstApplication.Names;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.ViewModels.Category
{
    public class CategoryViewModel
    {

        public CategoryViewModel()
        {

        }
        public CategoryViewModel(Domain.Models.Category category, string titleParent)
        {
            Id = category.CategoryId;
            Title = category.Title;
            TitleParent = titleParent;
        }
        public CategoryViewModel(Domain.Models.Category category, Domain.Models.Category categoryParent)
        {
            Id = category.CategoryId;
            Title = category.Title;
            TitleParent = categoryParent.Title;
            ParentId = categoryParent.CategoryId;
        }

        public int Id { set; get; }

        [Display(Name = DisplayNames.Title)]
        public string Title { set; get; }

        public int ParentId { set; get; }

        [Display(Name = DisplayNames.TitleParent)]
        public string TitleParent { set; get; }
    }
}
