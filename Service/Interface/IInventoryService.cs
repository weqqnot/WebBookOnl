namespace WebBookShell.Service.Interface
{
    public interface IInventoryService
    {
        Task AddBookToInven(int BookId, int Quantity);
        Task UpdateBookQuantity(int BookId, int Quantity);
        Task<int> GetBookQuantity(int BookId); // Lấy sách trong kho
        Task DeleteBookFromInven (int BookId);
    }
}
