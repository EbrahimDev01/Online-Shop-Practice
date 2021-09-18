using MyEshop.Application.ConstApplication.Names;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.ViewModels.Tag
{
    public class TagForSelect
    {
        public TagForSelect()
        {

        }
        public TagForSelect(Domain.Models.Tag tag)
        {
            Id = tag.TagId;
            Title = tag.Title;
            IsSelected = false;
        }

        public int Id { set; get; }

        [Display(Name = DisplayNames.Title)]
        public string Title { set; get; }

        public bool IsSelected { set; get; }
    }
}
