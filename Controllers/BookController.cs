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
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(new { message = "Danh sách sách đã được tải thành công", books });
        }

        // GET: api/Books/author/{authorName}
        [HttpGet("author/{authorName}")]
        public async Task<ActionResult> GetBooksByAuthor(string authorName)
        {
            var books = await _bookService.GetBooksByAuthorAsync(authorName);
            if (books == null || books.Count == 0)
            {
                return NotFound(new { message = $"Không tìm thấy sách của tác giả: {authorName}" });
            }
            return Ok(new { message = "Danh sách sách theo tác giả đã được tải thành công", books });
        }

        // GET: api/Books/genre/{genreName}
        [HttpGet("genre/{genreName}")]
        public async Task<ActionResult> GetBooksByGenre(string genreName)
        {
            var books = await _bookService.GetBooksByGenreAsync(genreName);
            if (books == null || books.Count == 0)
            {
                return NotFound(new { message = $"Không tìm thấy sách theo thể loại: {genreName}" });
            }
            return Ok(new { message = "Danh sách sách theo thể loại đã được tải thành công", books });
        }

        // GET: api/Books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookById(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                return Ok(new { message = "Thông tin sách đã được tải thành công", book });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Sách không tồn tại" });
            }
        }

        // GET: api/Books/{id}/Quantity
        [HttpGet("{id}/Quantity")]
        public async Task<ActionResult<int>> GetBookQuantity(int id)
        {
            try
            {
                var quantity = await _bookService.GetQuantityByIdAsync(id);
                return Ok(new { message = "Số lượng sách đã được tải thành công", quantity });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Sách không tồn tại" });
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
                Price = bookRequest.Price,
                Quantity = bookRequest.Quantity
            };

            var newBook = await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.BookId }, new { message = "Sách đã được thêm thành công", newBook });
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
                }, bookRequest.Quantity);
                return Ok(new { message = "Sách đã được cập nhật thành công", updatedBook });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Sách không tồn tại" });
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
                return NotFound(new { message = "Sách không tồn tại" });
            }
        }
    }
}
