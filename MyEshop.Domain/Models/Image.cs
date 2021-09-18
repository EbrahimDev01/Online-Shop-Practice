using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Models
{
    public class Image
    {

        public Image()
        {

        }
        public Image(string urlImage)
        {
            UrlImage = urlImage;
        }

        public int ImageId { set; get; }
        public string UrlImage { set; get; }


        public int ProductId { set; get; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { set; get; }
    }
}
