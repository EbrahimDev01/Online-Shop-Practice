using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.ViewModels.Image
{
    public class SelectImageToDelete
    {
        public SelectImageToDelete()
        {

        }
        public SelectImageToDelete(Domain.Models.Image image)
        {
            ImageId = image.ImageId;
            UrlImage = image.UrlImage;
        }

        public int ImageId { get; set; }
        public string UrlImage { get; set; }
        public bool IsSelected { get; set; }
    }
}
