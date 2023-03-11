using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {

        private RepositoryContext _context;
        private ICategoryRepository _categoryRepository;
        private ITagRepository _tagRepository;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_context);
                return _categoryRepository;
            }
        }

        public ITagRepository Tag {
     
            get
            {
                if (_tagRepository == null)
                    _tagRepository = new TagRepository(_context);
                return _tagRepository;
            }
}

public void Dispose() => _context.Dispose();

        public Task SaveAsync() => _context.SaveChangesAsync();

    }
}
