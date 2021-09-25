using MyEshop.Application.ConstApplication.Names;
using MyEshop.Application.Utilities.Convert;
using MyEshop.Application.ViewModels.Category;
using MyEshop.Application.ViewModels.Tag;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.ViewModels.Product
{
    public class ProductDetailsViewModel
    {
        public ProductDetailsViewModel()
        {

        }

        public ProductDetailsViewModel(Domain.Models.Product product, CategoryViewModel category,
            IEnumerable<Domain.Models.Tag> tags, IEnumerable<string> images, int commentCount)
        {
            #region Product Quantification

            ProductId = product.ProductId;
            Title = product.Title;
            Descritption = product.Descritption;
            Explanation = product.Explanation;
            Price = product.Price.ToString("0,0");
            QuantityInStok = product.QuantityInStok.ToString();
            CreateDateTime = product.CreateDateTime.ToString();
            CreateDateTimePersian = product.CreateDateTime.ToSolarHistory();
            Views = product.Views.ToString();

            #endregion

            Category = category;
            Tags = tags.Select(tag => new TagViewModel(tag));
            UrlImages = images;
            CommentCount = commentCount.ToString();
        }

        public int ProductId { set; get; }

        [Display(Name = DisplayNames.Title)]
        public string Title { get; set; }

        [Display(Name = DisplayNames.Descritption)]
        public string Descritption { set; get; }

        [Display(Name = DisplayNames.Explanation)]
        public string Explanation { get; set; }

        [Display(Name = DisplayNames.Price)]
        public string Price { set; get; }

        [Display(Name = DisplayNames.QuantityInStok)]
        public string QuantityInStok { set; get; }

        [Display(Name = DisplayNames.CreateDateTime)]
        public string CreateDateTime { set; get; }

        [Display(Name = DisplayNames.CreateDateTimePersian)]
        public string CreateDateTimePersian { set; get; }

        [Display(Name = DisplayNames.Views)]
        public string Views { set; get; }

        [Display(Name = DisplayNames.Views)]
        public string CommentCount { set; get; }

        #region Relations

        [Display(Name = DisplayNames.Category)]
        public CategoryViewModel Category { set; get; }

        [Display(Name = DisplayNames.Tags)]
        public IEnumerable<TagViewModel> Tags { set; get; } = new List<TagViewModel>();

        [Display(Name = DisplayNames.Images)]
        public IEnumerable<string> UrlImages { set; get; } = new List<string>();


        #endregion
    }
}
