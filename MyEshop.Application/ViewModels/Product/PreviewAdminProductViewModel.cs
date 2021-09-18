using MyEshop.Application.Utilities.Convert;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.ViewModels.Product
{
    public class PreviewAdminProductViewModel
    {
        public PreviewAdminProductViewModel()
        {

        }
        public PreviewAdminProductViewModel(Domain.Models.Product product, string imageUrl)
        {
            Id = product.ProductId;
            ImageUrl = imageUrl;
            Title = product.Title;
            Price = product.Price.ToString("0,0");
            QuantityInStok = product.QuantityInStok.ToString("0,0");
            CreateDateTime = product.CreateDateTime.ToString();
            CreateDateTimePersian = product.CreateDateTime.ToSolarHistory();
        }

        public int Id { set; get; }
        public string ImageUrl { set; get; }
        public string Title { set; get; }
        public string Price { set; get; }
        public string QuantityInStok { set; get; }
        public string CreateDateTime { set; get; }
        public string CreateDateTimePersian { set; get; }
    }
}
