using Microsoft.AspNetCore.Http;
using MyEshop.Application.Utilities.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MyEshop.Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace MyEshop.Test.UtilitiesTest.File
{
    public class FileHandlerTest
    {

        private readonly IFileHandler _fileHandler;

        public FileHandlerTest()
        {
            _fileHandler = new FileHandler();
        }


        #region test create file

        [Fact]
        public async Task Test_CreateAsync_Result_null()
        {
            string nameFile = await _fileHandler.CreateAsync(null);

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

            string newPath = Path.Combine("D:", "ebrahim", "Projects", "Web", "Web", "MyEshop", "MyEshop.Mvc", "wwwroot", "image");


            string nameFile = await _fileHandler.CreateAsync(file, newPath);


            string pahtFile = Path.Combine(newPath, nameFile);

            bool isExist = System.IO.File.Exists(pahtFile);

            Assert.True(isExist);

            System.IO.File.Delete(pahtFile);
        }

        #endregion

        #region test file delete

        [Fact]
        public void Test_File_Delete_Result_null()
        {
            bool isFileDeleted = _fileHandler.DeleteImages(new List<string>());

            Assert.True(isFileDeleted);
        }

        [Fact]
        public async Task Test_File_Delete_Result_Delete_File()
        {
            using var stream = System.IO.File.OpenRead(@"D:\ebrahim\wallpapers\download.jpg");

            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
            };
            string newPath = Path.Combine("D:", "ebrahim", "Projects", "Web", "Web", "MyEshop", "MyEshop.Mvc", "wwwroot", "image");

            string nameFile = await _fileHandler.CreateAsync(file, newPath);

            IEnumerable<string> images = new List<string>
            {
                nameFile
            };

            bool isFileDeleted = _fileHandler.DeleteImages(images, newPath);


            bool isExist = System.IO.File.Exists(images.FirstOrDefault());

            Assert.True(isFileDeleted);
            Assert.False(isExist);
        }

        #endregion

        #region Test is image

        [Fact]
        public void Test_Is_Image_Result_true()
        {
            using var stream = System.IO.File.OpenRead(@"D:\ebrahim\wallpapers\download.jpg");

            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
            };

            IEnumerable<IFormFile> images = new List<IFormFile>
            {
               file
            };

            bool isImage = _fileHandler.IsImage(images);

            Assert.True(isImage);
        }

        [Fact]
        public void Test_Is_Image_Result_false()
        {
            using var stream = System.IO.File.OpenRead(@"D:\ebrahim.html");

            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/html",
            };

            IEnumerable<IFormFile> images = new List<IFormFile>
            {
               file
            };

            bool isImage = _fileHandler.IsImage(images);

            Assert.False(isImage);
        }

        #endregion
    }
}
