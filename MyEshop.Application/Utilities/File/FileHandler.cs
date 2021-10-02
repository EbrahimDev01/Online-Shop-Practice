using Microsoft.AspNetCore.Http;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyEshop.Application.Utilities.File
{
    public class FileHandler : IFileHandler
    {
        public FileHandler()
        {

        }

        public async ValueTask<string> CreateAsync(IFormFile formFile, string newPath = null)
        {
            if (formFile == null)
                return null;

            try
            {
                string fileExtension = Path.GetExtension(formFile.FileName);
                string newFileName = Guid.NewGuid() + fileExtension;
                string newPathFile = "";

                if (string.IsNullOrEmpty(newPath))
                {
                    newPathFile = Path.Combine(Directory.GetCurrentDirectory(),
                                               "wwwroot",
                                               "image",
                                               newFileName);
                }
                else
                {
                    newPathFile = Path.Combine(newPath,
                                               "wwwroot",
                                               "image",
                                               newFileName);
                }

                using var fileStream = System.IO.File.Create(newPathFile);

                await formFile.CopyToAsync(fileStream);

                newPath = Path.Combine("/image/", newFileName);

                return newPath;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteImages(IEnumerable<Domain.Models.Image> images)
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

        public const int ImageMinimumBytes = 512;

        public bool IsImage(IEnumerable<IFormFile> formFiles)
        {
            foreach (var formFile in formFiles)
            {
                #region Check image

                //-------------------------------------------
                //  Check the image mime types
                //-------------------------------------------
                if (!string.Equals(formFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(formFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(formFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(formFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(formFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(formFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                //-------------------------------------------
                //  Check the image extension
                //-------------------------------------------
                var formFileExtension = Path.GetExtension(formFile.FileName);
                if (!string.Equals(formFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(formFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(formFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(formFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                //-------------------------------------------
                //  Attempt to read the file and check the first bytes
                //-------------------------------------------
                try
                {
                    if (!formFile.OpenReadStream().CanRead)
                    {
                        return false;
                    }
                    //------------------------------------------
                    //   Check whether the image size exceeding the limit or not
                    //------------------------------------------ 
                    if (formFile.Length < ImageMinimumBytes)
                    {
                        return false;
                    }

                    byte[] buffer = new byte[ImageMinimumBytes];
                    formFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                    string content = System.Text.Encoding.UTF8.GetString(buffer);
                    if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                //-------------------------------------------
                //  Try to instantiate new Bitmap, if .NET will throw exception
                //  we can assume that it's not a valid image
                //-------------------------------------------

                try
                {
                    using var bitmap = new Bitmap(formFile.OpenReadStream());
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    formFile.OpenReadStream().Position = 0;
                }
                #endregion
            }
            return true;
        }
    }
}
