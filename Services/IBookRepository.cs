using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public interface IBookRepository
    {
        //get all books 
        ICollection<Book> GetBooks();

        //get specific book by id 
        Book GetBook(int bookId);

        bool BookExist(int bookId);

        bool CreatedBook(Book book);
        bool UpdateBook(Book book);
        bool DeleteBook(Book book);
        bool Save();

        bool IsDuplicatedBookId(int bookId, string bookTitle);
        

    }
}
