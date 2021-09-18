using MyEshop.Domain.ConstsDomain.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Models
{
    public class Category
    {
        public Category()
        {

        }

        public Category(string title)
        {
            Title = title;
        }

        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(150, ErrorMessage = ErrorMessage.MaxLength)]
        public string Title { get; set; }


        #region Relations

        [Display(Name = "دسته بندی پدر")]
        public virtual int? CategoryParentId { set; get; }
        [ForeignKey(nameof(CategoryParentId))]
        public virtual Category CategoryParent { set; get; }
        public virtual ICollection<Category> CategoriesChildren { set; get; } = new HashSet<Category>();


        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

        #endregion
    }
}
