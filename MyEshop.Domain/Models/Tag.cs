using MyEshop.Domain.ConstsDomain.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Models
{
    public class Tag
    {

        public Tag()
        {

        }
        public Tag(string title)
        {
            Title = title;
        }

        [Key]
        public int TagId { get; set; }

        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(150, ErrorMessage = ErrorMessage.MaxLength)]
        public string Title { get; set; }


        #region Relations

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

        #endregion
    }
}
