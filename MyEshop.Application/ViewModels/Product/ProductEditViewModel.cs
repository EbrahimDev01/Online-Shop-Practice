using Microsoft.AspNetCore.Http;
using MyEshop.Application.ConstApplication.Names;
using MyEshop.Application.ViewModels.Category;
using MyEshop.Application.ViewModels.Image;
using MyEshop.Application.ViewModels.Tag;
using MyEshop.Domain.ConstsDomain.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.ViewModels.Product
{
    public class ProductEditViewModel
    {
        public ProductEditViewModel()
        {

        }
        public ProductEditViewModel(IAsyncEnumerable<CategoryViewModel> categories, IList<TagForSelect> tagsForSelect)
        {
            Categories = categories;
            Tags = tagsForSelect;
        }

        [Display(Name = DisplayNames.Title)]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(150, ErrorMessage = ErrorMessage.MaxLength)]
        public string Title { get; set; }

        [Display(Name = DisplayNames.Descritption)]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [StringLength(250, MinimumLength = 10, ErrorMessage = ErrorMessage.StringLength)]
        [DataType(DataType.MultilineText)]
        public string Descritption { set; get; }

        [Display(Name = DisplayNames.Explanation)]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [DataType(DataType.MultilineText)]
        public string Explanation { get; set; }

        [Display(Name = DisplayNames.Price)]
        public int Price { set; get; }

        [Display(Name = DisplayNames.QuantityInStok)]
        public int QuantityInStok { set; get; }

        #region Relations

        [Display(Name = DisplayNames.SelectCategory)]
        [Required(ErrorMessage = ErrorMessage.Required)]
        public int CategoryId { get; set; }

        public virtual IAsyncEnumerable<CategoryViewModel> Categories { set; get; }

        [Display(Name = DisplayNames.Tags)]
        public virtual IList<TagForSelect> Tags { set; get; } = new List<TagForSelect>();

        [Display(Name = DisplayNames.Images)]
        public IList<IFormFile> Images { set; get; }


        [Display(Name = DisplayNames.AvailableImages)]
        public virtual IList<SelectImageToDelete> AvailableImages { set; get; } = new List<SelectImageToDelete>();
        #endregion
    }
}
