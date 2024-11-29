using WebBookShell.Entities;

namespace WebBookShell.Service.Interface
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<List<Book>> GetBooksByAuthorAsync(string authorName);
        Task<List<Book>> GetBooksByGenreAsync(string genreName);
        Task<Book> GetBookByIdAsync(int id);
        Task<int> GetQuantityByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task<Book> UpdateBookAsync(int id, Book book, int Quantity);

        Task DeleteBookAsync(int id);
    }
}
