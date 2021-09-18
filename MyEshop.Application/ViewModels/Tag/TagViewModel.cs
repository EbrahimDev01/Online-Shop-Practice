using MyEshop.Application.ConstApplication.Names;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.ViewModels.Tag
{
    public class TagViewModel
    {

        public TagViewModel()
        {

        }
        public TagViewModel(Domain.Models.Tag tag)
        {
            Id = tag.TagId;
            Title = tag.Title;
        }

        public int Id { set; get; }

        [Display(Name = DisplayNames.Title)]
        public string Title { set; get; }
    }
}
