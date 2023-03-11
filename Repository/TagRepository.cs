using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Parameters;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateTag(Tag tag) => Create(tag);

        public void DeleteTag(Tag tag) => Delete(tag);

        public async Task<PagedList<Tag>> GetAllTagsAsync(TagParameters tagParameters, bool trackChanges)
        {
            var tags = await FindAll(trackChanges)
                .Search(tagParameters.SearchTerm)
                .Sort(tagParameters.OrderBy)
                .ToListAsync();
            return PagedList<Tag>.ToPagedList(tags, tagParameters.PageNumber, tagParameters.PageSize);
        }


        public async Task<Tag> GetTagsByIdAsync(Guid tagId, bool trackChanges)
        {
            var tag = await FindByCondition(c => c.Id.Equals(tagId), trackChanges).SingleOrDefaultAsync();
            return tag;
        }

        public void UpdateTag(Tag tag) => Update(tag);
    }
}
