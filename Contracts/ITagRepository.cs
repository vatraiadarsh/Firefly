using Entities.Models;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITagRepository : IRepositoryBase<Tag>
    {
        Task<PagedList<Tag>> GetAllTagsAsync(TagParameters tagParameters, bool trackChanges);
        Task<Tag> GetTagsByIdAsync(Guid tagId, bool trackChanges);
        void CreateTag(Tag tag);
        void UpdateTag(Tag tag);
        void DeleteTag(Tag tag);
    }
}
