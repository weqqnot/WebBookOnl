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

        public async Task TransferToSale(int BookId, int Quantity)
        {
            var BookInInventory = await _context.Inventories.FirstOrDefaultAsync(b => b.BookId == BookId);

            if (BookInInventory == null || BookInInventory.Quantity < Quantity)
            {
                throw new InvalidOperationException("Insufficient stock to transfer.");
            }

            // Giảm số lượng sách trong kho
            BookInInventory.Quantity -= Quantity;

            // Thêm sách vào hệ thống bán hàng
            var BookForSale = new BookForSale
            {
                BookId = BookInInventory.BookId,
                Quantity = Quantity,
            };

            _context.BookForSale.Add(BookForSale);

            await _context.SaveChangesAsync();
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
        public async Task<List<Inventory>> GetInvenInfoAsynnc()
        {
            var inventoryInfo = await _context.Inventories
                .Include(i => i.Books)  // Lấy thông tin sách từ bảng Book
                .Include(i => i.BookForSale)  // Lấy thông tin bán sách từ bảng BookForSale
                .ToListAsync();

            var result = inventoryInfo.Select(i => new Inventory
            {
                BookId = i.BookId,
                CostPrice = i.CostPrice,
                QuantityForSale = i.BookForSale.Sum(bfs => bfs.Quantity) // Tổng số lượng sách đã chuyển sang bán
            }).ToList();

            return result;
        }
    }

}
