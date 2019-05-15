using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Dtos;
using BookApiProject.Models;
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
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn
                });
            }
            return Ok(bookDto);
        }

        [HttpGet("{bookId}", Name = "GetBook")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBook(int bookId)
        {
            if (!_bookRepository.BookExist(bookId))
                return NotFound();

            var book  = _bookRepository.GetBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDto = new BookDto()
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn
            };

            return Ok(bookDto);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody]Book bookToCreate)
        {
            if (bookToCreate == null)
                return BadRequest(ModelState);

            var book = _bookRepository.GetBooks().Where(b => b.Title.Trim().ToUpper() == bookToCreate.Title.Trim().ToUpper())
                .FirstOrDefault();

            if(book != null)
            {
                ModelState.AddModelError("", $"Book {bookToCreate.Title} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_bookRepository.CreatedBook(bookToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {bookToCreate.Title}");
                return StatusCode(500, ModelState);

            }

            return CreatedAtRoute("GetBook", new { bookId = bookToCreate.Id }, bookToCreate);

        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateBook(int bookId, [FromBody]Book updatedBookInfo)
        {
            if (updatedBookInfo == null)
                return NotFound(ModelState);

            if (bookId != updatedBookInfo.Id)
                return BadRequest(ModelState);

            if (!_bookRepository.BookExist(bookId))
                return NotFound();

            if(_bookRepository.IsDuplicatedBookId(bookId, updatedBookInfo.Title))
            {
                ModelState.AddModelError("", $"Book {updatedBookInfo.Title} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_bookRepository.UpdateBook(updatedBookInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating {updatedBookInfo.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


        [HttpDelete("{bookId}")]
        public IActionResult DeleteBook(int bookId)
        {
            if (!_bookRepository.BookExist(bookId))
                return NotFound();

            var bookToDelete = _bookRepository.GetBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_bookRepository.DeleteBook(bookToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {bookToDelete.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}