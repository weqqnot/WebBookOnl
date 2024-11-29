using Microsoft.EntityFrameworkCore;
using WebBookShell.Data;
using WebBookShell.DTO;
using WebBookShell.Entities;
using WebBookShell.Service.Interface;

namespace WebBookShell.Service.Implement
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Inventory> AddBookAsync(Inventory inventory)
        {
            // Kiểm tra xem sách có tồn tại không (có thể dựa trên tiêu đề, hoặc các điều kiện khác)
            var existingBook = await _context.Inventories
                .FirstOrDefaultAsync(b => b.Title == inventory.Title);

            if (existingBook != null)
            {
                throw new InvalidOperationException("Book with this title already exists.");
            }

            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task<Inventory> UpdateBookAsync(int InventoryBookId, Inventory updatedInventory, int quantityChange)
        {
            var inventory = await _context.Inventories.FindAsync(InventoryBookId);
            if (inventory == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }
            
            // Cập nhật thông tin sách
            inventory.Title = updatedInventory.Title;
            inventory.CostPrice = updatedInventory.CostPrice;

            inventory.Quantity += quantityChange;

            if (inventory.Quantity < 0)
            {
                throw new InvalidOperationException("Số lượng sách không thể âm.");
            }
            _context.Inventories.Update(inventory);
            // Lưu thay đổi
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task DelteBookAsync(int InventoryBookId)
        {
            var inventory = await _context.Inventories.FindAsync(InventoryBookId);
            if (inventory == null) throw new KeyNotFoundException("Book not found.");

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
        }
        public async Task<Inventory> GetInventoryByBookIdAsync(int InventoryBookId)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.InventoryBookId == InventoryBookId);

            if (inventory == null)
            {
                throw new KeyNotFoundException("Book not found in inventory.");
            }

            return inventory;
        }
        public async Task<List<Inventory>> GetAllInventoryAsync()
        {
            return await _context.Inventories.ToListAsync();
        }
    }
}
