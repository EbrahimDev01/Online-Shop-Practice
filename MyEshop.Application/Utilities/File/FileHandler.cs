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


        public async ValueTask<string> CreateAsync(IFormFile formFile, string pathFile = null)
        {
            if (formFile is null)
            {
                return null;
            }


            string fileExtension = Path.GetExtension(formFile.FileName);
            string newFileName = Guid.NewGuid() + fileExtension;

            string pathAndNameFile = CombinePahtAndNameFile(newFileName, pathFile);

            try
            {
                using var fileStream = System.IO.File.Create(pathAndNameFile);

                await formFile.CopyToAsync(fileStream);

                return newFileName;
            }
            catch
            {
                return null;
            }
        }


        public bool DeleteImages(IEnumerable<string> namesFile, string pathFile = null)
        {
            if (namesFile is null)
                return true;

            try
            {
                foreach (var nameFile in namesFile)
                {
                    string fullPathFile = CombinePahtAndNameFile(nameFile, pathFile);

                    System.IO.File.Delete(fullPathFile);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public const int ImageMinimumBytes = 512;


        public string CombinePahtAndNameFile(string fileName, string pathFile = null)
            => Path.Combine(pathFile ?? IFileHandler.ImageFilePath, fileName);

        public bool IsImage(IEnumerable<IFormFile> formFiles)
        {
            if (formFiles is not null)
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
            }

            return true;
        }
    }
}
