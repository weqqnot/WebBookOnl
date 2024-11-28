using Microsoft.EntityFrameworkCore;
using WebBookShell.Data;
using WebBookShell.Entities;
using WebBookShell.Service.Interface;
using System.Threading.Tasks;

namespace WebBookShell.Service
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        // L?y t?t c? sách
        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .Include(b => b.Inventories)
                .ToListAsync();
        }

        // L?y sách theo Author
        public async Task<List<Book>> GetBooksByAuthorAsync(string authorName)
        {
            return await _context.Books
                .Where(b => b.AuthorName == authorName)
                .ToListAsync();
        }

        // L?y sách theo Genre
        public async Task<List<Book>> GetBooksByGenreAsync(string genreName)
        {
            return await _context.Books
                .Where(b => b.GenreName == genreName)
                .ToListAsync();
        }

        // L?y sách theo ID
        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .Include(b => b.Inventories)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null) throw new KeyNotFoundException("Book not found.");

            return book;
        }

        // Thêm sách
        public async Task<Book> AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        // C?p nh?t sách
        public async Task<Book> UpdateBookAsync(int id, Book book)
        {
            var existingBook = await _context.Books.FindAsync(id);

            if (existingBook == null) throw new KeyNotFoundException("Book not found.");

            existingBook.Title = book.Title;
            existingBook.Description = book.Description;
            existingBook.Price = book.Price;
            existingBook.AuthorName = book.AuthorName;
            existingBook.GenreName = book.GenreName;

            await _context.SaveChangesAsync();

            return existingBook;
        }

        // Xóa sách
        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) throw new KeyNotFoundException("Book not found.");

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
