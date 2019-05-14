using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Models;

namespace BookApiProject.Services
{
    public class BookRepository : IBookRepository
    {
        private BookDbContext _bookContext;

        public BookRepository(BookDbContext bookContext)
        {
            _bookContext = bookContext;
        }

        public bool BookExist(int bookId)
        {
            return _bookContext.Books.Any(b => b.Id == bookId);
        }

        public Book GetBook(int bookId)
        {
            return _bookContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public ICollection<Book> GetBooks()
        {
            return _bookContext.Books.OrderBy(b => b.Title).ToList();
        }
    }
}
