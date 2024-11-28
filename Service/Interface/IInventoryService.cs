using WebBookShell.Entities;

namespace WebBookShell.Service.Interface
{
    public interface IInventoryService
    {
        Task<List<Inventory>> GetInvenInfoAsynnc();
        Task AddBookToInven(int BookId, int Quantity);
        Task UpdateBookQuantity(int BookId, int Quantity);
        Task<int> GetBookQuantity(int BookId); // Lấy sách trong kho
        Task TransferToSale(int BookId, int Quantity);
        Task DeleteBookFromInven (int BookId);
    }
}
