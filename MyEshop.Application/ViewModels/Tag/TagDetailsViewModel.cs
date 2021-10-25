using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.ViewModels.Tag
{
    public class TagDetailsViewModel : TagViewModel
    {
        public TagDetailsViewModel()
        {

        }

        public TagDetailsViewModel(Domain.Models.Tag tag, IAsyncEnumerable<Product.PreviewAdminProductViewModel> previewAdminProducts)
            : base(tag)
        {
            PreviewAdminProducts = previewAdminProducts;
        }

        public IAsyncEnumerable<Product.PreviewAdminProductViewModel> PreviewAdminProducts { set; get; }
    }
}
