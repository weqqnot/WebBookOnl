using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBookShell.DTOs;
using WebBookShell.Entities;
using WebBookShell.Service.Interface;
using System.Threading.Tasks;

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
            return Ok(books);
        }

        // GET: api/Books/author/{authorName}
        [HttpGet("author/{authorName}")]
        public async Task<ActionResult> GetBooksByAuthor(string authorName)
        {
            var books = await _bookService.GetBooksByAuthorAsync(authorName);
            return Ok(books);
        }

        // GET: api/Books/genre/{genreName}
        [HttpGet("genre/{genreName}")]
        public async Task<ActionResult> GetBooksByGenre(string genreName)
        {
            var books = await _bookService.GetBooksByGenreAsync(genreName);
            return Ok(books);
        }

        // GET: api/Books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookById(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                return Ok(book);
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
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.BookId }, newBook);
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
                return Ok(updatedBook);
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
