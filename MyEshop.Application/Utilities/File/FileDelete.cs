using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.Utilities.File
{
    public class FileDelete
    {
        public static bool Delete(IEnumerable<Image> images)
        {
            try
            {
                foreach (var image in images)
                {
                    string urlImage = image.UrlImage.Remove(0, 7);

                    string pathFile = Path.Combine(Directory.GetCurrentDirectory(),
                                                       "wwwroot", "image", urlImage);

                    System.IO.File.Delete(pathFile);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
