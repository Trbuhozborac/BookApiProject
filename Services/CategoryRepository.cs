using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Models;

namespace BookApiProject.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private BookDbContext _categoryContext;

        public CategoryRepository(BookDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }

        public bool CategoryExist(int categoryId)
        {
            return _categoryContext.Categories.Any(c => c.Id == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            _categoryContext.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _categoryContext.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _categoryContext.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _categoryContext.SaveChanges();
            return saved >= 0 ? true : false;
            
        }
        

        public bool UpdateCategory(Category category)
        {
            _categoryContext.Update(category);
            return Save();
        }
    }
}
