using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Dtos;
using BookApiProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetBooks()
        {
            var books = _bookRepository.GetBooks().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDto = new List<BookDto>();

            foreach (var book in books)
            {
                bookDto.Add(new BookDto
                {
                    Title = book.Title,
                    Isbn = book.Isbn
                });
            }
            return Ok(bookDto);
        }

        [HttpGet("{bookId}")]
        public IActionResult GetBook(int bookId)
        {
            if (!_bookRepository.BookExist(bookId))
                return NotFound();

            var book  = _bookRepository.GetBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDto = new BookDto()
            {
                Title = book.Title,
                Isbn = book.Isbn
            };

            return Ok(bookDto);
        }


    }
}