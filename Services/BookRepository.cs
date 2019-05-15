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

        public bool CreatedBook(Book book)
        {
            _bookContext.Add(book);
            return Save();

        }

        public bool DeleteBook(Book book)
        {
            _bookContext.Remove(book);
            return Save();
        }

      

        public Book GetBook(int bookId)
        {
            return _bookContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public ICollection<Book> GetBooks()
        {
            return _bookContext.Books.OrderBy(b => b.Title).ToList();
        }

        

        public bool IsDuplicatedBookId(int bookId, string bookTitle)
        {
            var book = _bookContext.Books.Where(b => b.Title.Trim().ToUpper() == bookTitle.Trim().ToUpper() && b.Id != bookId).FirstOrDefault();
            return book == null ? false : true;
        }

        public bool Save()
        {
            var saved = _bookContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateBook(Book book)
        {
            _bookContext.Update(book);
            return Save();
        }
    }
}
