using WebBookShell.DTO;
using WebBookShell.Entities;

namespace WebBookShell.Service.Interface
{
    public interface IInventoryService
    {
        Task<List<Inventory>> GetAllInventoryAsync();
        Task<Inventory> AddBookAsync(Inventory inventory);
        Task<Inventory> UpdateBookAsync(int InventoryBookId, Inventory inventory, int quantityChange);
        Task DelteBookAsync( int InventoryBookId);
        Task<Inventory> GetInventoryByBookIdAsync(int bookId); //lấy thông tin về số lượng sách trong kho cho một cuốn sách

    }
}
