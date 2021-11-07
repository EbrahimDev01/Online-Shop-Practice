
namespace MyEshop.Application.ViewModels.Tag
{
    public class TagDeleteViewModel : TagViewModel
    {
        public TagDeleteViewModel()
        {

        }
        public TagDeleteViewModel(Domain.Models.Tag tag)
            : base(tag)
        {
        }
    }
}
