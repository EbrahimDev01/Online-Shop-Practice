using Microsoft.AspNetCore.Http;
using MyEshop.Application.Utilities.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyEshop.Test.UtilitiesTest.File
{
    public class CreateFileTest
    {
        [Fact]
        public async Task Test_CreateAsync_Result_null()
        {
            string nameFile = await CreateFile.CreateAsync(null);

            Assert.Null(nameFile);
        }

        [Fact]
        public async Task Test_FileCreate_CreateAsync_Result_new_Name_and_Create_File()
        {

            using var stream = System.IO.File.OpenRead(@"D:\ebrahim\wallpapers\download.jpg");

            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
            };

            string newPath = Path.Combine("D:", "", "ebrahim", "Projects", "Web", "Web", "MyEshop", "MyEshop.Mvc");


            string nameFile = await CreateFile.CreateAsync(file, newPath);

            nameFile = nameFile.Remove(0, 7);

            string pahtFile = Path.Combine(newPath,
                                               "wwwroot", "image", nameFile);

            bool isExist = System.IO.File.Exists(pahtFile);

            Assert.True(isExist);

            System.IO.File.Delete(nameFile);
        }
    }
}
