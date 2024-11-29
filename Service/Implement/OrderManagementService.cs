using WebBookShell.DTO;
using WebBookShell.Entities;
using Microsoft.EntityFrameworkCore;
using WebBookShell.Data;
using WebBookShell.Interfaces;
using WebBookShell.Exception;

namespace WebBookShell.Services
{
    public class OrderManagementService : IOrderManagementService
    {
        private readonly ApplicationDbContext _context;

        public OrderManagementService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả đơn hàng
        public async Task<IEnumerable<AllOrdersDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Users)  // Lấy thông tin người dùng
                .Select(o => new AllOrdersDto
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    UserName = o.Users.Fullname, // Sử dụng thuộc tính Fullname của User
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount
                })
                .ToListAsync();

            return orders;
        }

        // Lấy tổng số lượng đơn hàng
        public async Task<int> GetTotalOrdersCountAsync()
        {
            return await _context.Orders.CountAsync();
        }

        // Lấy chi tiết một đơn hàng
        public async Task<OrderDto> GetOrderDetailsAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Books) // Lấy thông tin sách từ OrderDetail
                .Include(o => o.Users) // Lấy thông tin người dùng
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) throw new UserException("Order not found.");

            var orderDto = new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                UserName = order.Users.Fullname, // Sử dụng Fullname từ bảng Users
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    BookId = od.BookId,
                    Title = od.Books.Title, // Lấy Title từ bảng Book
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            };

            return orderDto;
        }
    }
}
