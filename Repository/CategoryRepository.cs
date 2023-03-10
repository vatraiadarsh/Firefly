using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Parameters;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System.Linq.Expressions;

namespace Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateCategory(Category category) => Create(category);
        public void DeleteCategory(Category category) => Delete(category);

        public async Task<PagedList<Category>> GetAllCategoriesAsync(CategoryParameters categoryParameters, bool trackChanges)
        {
            var categories = await FindAll(trackChanges)
                .Search(categoryParameters.SearchTerm)
                .Sort(categoryParameters.OrderBy)
                .ToListAsync();
            return PagedList<Category>.ToPagedList(categories, categoryParameters.PageNumber, categoryParameters.PageSize);
        }

        public async Task<Category> GetCategoryByIdAsync(Guid categoryId, bool trackChanges)
        {
            var category = await FindByCondition(c => c.Id.Equals(categoryId), trackChanges).SingleOrDefaultAsync();
            return category;
        }

        public void UpdateCategory(Category category) => Update(category);
    }
}