using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges)
        {
           var categories  = await FindAll(trackChanges)
                .OrderBy(c => c.Name)
                .ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(Guid categoryId, bool trackChanges)
        {
            var category = await FindByCondition(c => c.Id.Equals(categoryId), trackChanges).SingleOrDefaultAsync();
            return category;
        }

        public void UpdateCategory(Category category) => Update(category);
    }
}