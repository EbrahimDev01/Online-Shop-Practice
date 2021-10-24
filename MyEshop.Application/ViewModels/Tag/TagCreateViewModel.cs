using Microsoft.AspNetCore.Mvc;
using MyEshop.Application.ConstApplication.Names;
using MyEshop.Application.Utilities.ConstApplication.Names;
using MyEshop.Domain.ConstsDomain.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.ViewModels.Tag
{
    public class TagCreateViewModel
    {
        [Display(Name = DisplayNames.Title)]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(150, ErrorMessage = ErrorMessage.MaxLength)]
        [Remote("IsExistTagByTitle", "TagManager", "Admin",
            HttpMethod = "POST",
            AdditionalFields = MostUsedNames.RequestVerificationToken,
            ErrorMessage = ErrorMessage.IsExist)]
        public string Title { get; set; }
    }
}
