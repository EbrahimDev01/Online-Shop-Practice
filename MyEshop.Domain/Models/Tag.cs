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

        public int TagId { get; set; }
        public string Title { get; set; }

        #region Relations

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

        #endregion
    }
}
