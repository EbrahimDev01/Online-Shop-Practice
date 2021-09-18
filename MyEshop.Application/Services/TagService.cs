using MyEshop.Application.Interfaces;
using MyEshop.Application.ViewModels.Tag;
using MyEshop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async ValueTask<IList<TagForSelect>> GetTagsForSelectAsync()
            =>await _tagRepository.GetTagsAsync().Select(t => new TagForSelect(t)).ToListAsync();
    }
}
