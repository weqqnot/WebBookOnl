using WebBookShell.Service.Interface;
using WebBookShell.Entities;
using Microsoft.EntityFrameworkCore;
using WebBookShell.Data;
using System.Net;

namespace WebBookShell.Service.Implement
{
    public class InventoryService : IInventoryService
    {
    
        private readonly ApplicationDbContext _context;
        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddBookToInven(int BookId, int Quantity)
        {
            var existingInventory = await _context.Inventories.FirstOrDefaultAsync(i => i.BookId == BookId);

            if (existingInventory != null)
            {
                throw new InvalidOperationException("Sách đã tồn tại trong kho.");
            }

            var newInventory = new Inventory
            {
                BookId = BookId,
                Quantity = Quantity
            };

            _context.Inventories.Add(newInventory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookQuantity(int BookId, int Quantity)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.BookId == BookId);

            if (inventory == null)
            {
                throw new KeyNotFoundException("Sách không tồn tại trong kho.");
            }

            inventory.Quantity += Quantity;

            if (inventory.Quantity < 0)
            {
                throw new InvalidOperationException("Số lượng sách không thể âm.");
            }

            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetBookQuantity(int BookId)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.BookId == BookId);

            if (inventory == null)
            {
                throw new KeyNotFoundException("Sách không tồn tại trong kho.");
            }

            return inventory.Quantity;
        }

        public async Task DeleteBookFromInven(int BookId)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.BookId == BookId);

            if (inventory == null)
            {
                throw new KeyNotFoundException("Sách không tồn tại trong kho.");
            }

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
        }
    }
}
