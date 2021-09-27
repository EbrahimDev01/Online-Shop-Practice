using Microsoft.AspNetCore.Http;
using MyEshop.Application.Utilities.File;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyEshop.Test.UtilitiesTest.File
{
    public class FileDeleteTest
    {
        [Fact]
        public async Task Test_File_Delete_Result_Delete_File()
        {
            using var stream = System.IO.File.OpenRead(@"D:\ebrahim\wallpapers\download.jpg");

            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
            };
            string newPath = Path.Combine("D:", "ebrahim", "Projects", "Web", "Web", "MyEshop", "MyEshop.Mvc");


            string nameFile = await FileCreate.CreateAsync(file, newPath);

            nameFile = nameFile.Remove(0, 7);

            IEnumerable<Image> images = new List<Image>
            {
                new Image
                {
                    UrlImage= Path.Combine("image", nameFile)
                }
            };

            bool isFileDeleted = FileDelete.Delete(images);


            bool isExist = System.IO.File.Exists(images.FirstOrDefault().UrlImage);

            Assert.False(isExist);
        }
    }
}
