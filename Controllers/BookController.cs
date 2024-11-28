using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBookShell.DTOs;
using WebBookShell.Entities;
using WebBookShell.Service.Interface;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebBookShell.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();

            // Chỉ trả về các trường cần thiết
            var response = books.Select(book => new
            {
                book.BookId,
                book.Title,
                book.Description,
                book.AuthorName,
                book.GenreName,
                book.Price
            });

            return Ok(response);
        }

        // GET: api/Books/author/{authorName}
        [HttpGet("author/{authorName}")]
        public async Task<ActionResult> GetBooksByAuthor(string authorName)
        {
            var books = await _bookService.GetBooksByAuthorAsync(authorName);

            // Chỉ trả về các trường cần thiết
            var response = books.Select(book => new
            {
                book.BookId,
                book.Title,
                book.Description,
                book.AuthorName,
                book.GenreName,
                book.Price
            });

            return Ok(response);
        }

        // GET: api/Books/genre/{genreName}
        [HttpGet("genre/{genreName}")]
        public async Task<ActionResult> GetBooksByGenre(string genreName)
        {
            var books = await _bookService.GetBooksByGenreAsync(genreName);

            // Chỉ trả về các trường cần thiết
            var response = books.Select(book => new
            {
                book.BookId,
                book.Title,
                book.Description,
                book.AuthorName,
                book.GenreName,
                book.Price
            });

            return Ok(response);
        }

        // GET: api/Books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookById(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);

                // Chỉ trả về các trường cần thiết
                var response = new
                {
                    book.BookId,
                    book.Title,
                    book.Description,
                    book.AuthorName,
                    book.GenreName,
                    book.Price
                };

                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Books
        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult> AddBook(BookRequest bookRequest)
        {
            var book = new Book
            {
                Title = bookRequest.Title,
                Description = bookRequest.Description,
                AuthorName = bookRequest.AuthorName,
                GenreName = bookRequest.GenreName,
                Price = bookRequest.Price
            };

            var newBook = await _bookService.AddBookAsync(book);

            // Chỉ trả về các trường cần thiết
            var response = new
            {
                newBook.BookId,
                newBook.Title,
                newBook.Description,
                newBook.AuthorName,
                newBook.GenreName,
                newBook.Price
            };

            return CreatedAtAction(nameof(GetBookById), new { id = response.BookId }, response);
        }

        // PUT: api/Books/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult> UpdateBook(int id, BookRequest bookRequest)
        {
            try
            {
                var updatedBook = await _bookService.UpdateBookAsync(id, new Book
                {
                    Title = bookRequest.Title,
                    Description = bookRequest.Description,
                    AuthorName = bookRequest.AuthorName,
                    GenreName = bookRequest.GenreName,
                    Price = bookRequest.Price
                });

                // Chỉ trả về các trường cần thiết
                var response = new
                {
                    updatedBook.BookId,
                    updatedBook.Title,
                    updatedBook.Description,
                    updatedBook.AuthorName,
                    updatedBook.GenreName,
                    updatedBook.Price
                };

                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Books/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return NoContent(); // HTTP 204 No Content
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
