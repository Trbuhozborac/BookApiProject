using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);

        bool CategoryExist(int categoryId);

        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();


    }
}
