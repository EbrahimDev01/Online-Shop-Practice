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
    public class ProductEditViewModel : ProductCreateViewModel
    {
        public ProductEditViewModel()
        {

        }
        public ProductEditViewModel(IAsyncEnumerable<CategoryViewModel> categories, IList<TagForSelect> tagsForSelect,
            IList<SelectImageToDelete> availableImages) : base(categories, tagsForSelect)
        {
            AvailableImages = availableImages;
        }

        public int ProductId { set; get; }

        [Display(Name = DisplayNames.Price)]
        new public int Price { set; get; }

        [Display(Name = DisplayNames.QuantityInStok)]
        new public int QuantityInStok { set; get; }

        #region Relations

        [Display(Name = DisplayNames.AvailableImages)]
        public IList<SelectImageToDelete> AvailableImages { set; get; } = new List<SelectImageToDelete>();

        #endregion
    }
}
