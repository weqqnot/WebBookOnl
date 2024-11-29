    using Microsoft.EntityFrameworkCore;
    using WebBookShell.Data;
    using WebBookShell.DTO;
    using WebBookShell.Entities;
    using WebBookShell.Exception;
    using WebBookShell.Interfaces;

    namespace WebBookShell.Services
    {
        public class CartService : ICartService
        {
            private readonly ApplicationDbContext _context;

            public CartService(ApplicationDbContext context)
            {
                _context = context;
            }

            // Thêm sách vào giỏ hàng
            public async Task AddToCartAsync(int userId, CartRequest request)
            {
                // Lấy giỏ hàng của user
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    // Nếu chưa có giỏ hàng, tạo mới giỏ hàng
                    cart = new Cart { UserId = userId };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                }

                // Kiểm tra xem sách có tồn tại không
                var book = await _context.Books.FindAsync(request.BookId);
                if (book == null) throw new UserException("Book not found");

                // Kiểm tra xem sách đã có trong giỏ chưa
                var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == request.BookId);
                if (existingCartItem != null)
                {
                    // Nếu có rồi, chỉ cần cập nhật số lượng
                    existingCartItem.Quantity += request.Quantity;
                }
                else
                {
                    // Nếu chưa có, thêm sản phẩm mới vào giỏ hàng
                    cart.CartItems.Add(new CartItem
                    {
                        BookId = request.BookId,
                        Quantity = request.Quantity
                    });
                }

                await _context.SaveChangesAsync();
            }

            // Cập nhật số lượng sách trong giỏ hàng
            public async Task UpdateQuantityAsync(int userId, int bookId, int quantity)
            {
                // Tìm giỏ hàng của người dùng
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null) throw new UserException("Cart not found");

                // Tìm sản phẩm trong giỏ hàng
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);
                if (cartItem == null) throw new UserException("Cart item not found");

                // Kiểm tra số lượng hợp lệ
                if (quantity == 0)
                {
                    throw new UserException("Quantity must not be zero.");
                }

                // Cập nhật số lượng sản phẩm, cộng hoặc trừ tùy vào giá trị quantity
                cartItem.Quantity += quantity;

                // Đảm bảo số lượng không nhỏ hơn 1
                if (cartItem.Quantity < 1)
                {
                    cartItem.Quantity = 1;  // Giới hạn số lượng tối thiểu là 1
                }

                // Lưu lại thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }

            // Xóa sách khỏi giỏ hàng
            public async Task RemoveFromCartAsync(int userId, int bookId)
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null) throw new UserException("Cart not found");

                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);
                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new UserException("Cart item not found");
                }
            }

       
            // Lấy tất cả thông tin chi tiết sách trong giỏ hàng của người dùng
            public async Task<IEnumerable<BookDetails>> GetAllBooksDetailsInCartAsync(int userId)
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null) throw new UserException("Cart not found");

                // Lấy danh sách tất cả sách trong giỏ
                var bookIds = cart.CartItems.Select(ci => ci.BookId).ToList();
                var books = await _context.Books.Where(b => bookIds.Contains(b.BookId)).ToListAsync();

                // Danh sách để chứa thông tin chi tiết sách
                var bookDetailsList = new List<BookDetails>();

                foreach (var item in cart.CartItems)
                {
                    var book = books.FirstOrDefault(b => b.BookId == item.BookId);
                    if (book != null)
                    {
                        bookDetailsList.Add(new BookDetails
                        {
                            BookId = book.BookId,
                            Title = book.Title,
                            AuthorName = book.AuthorName,
                            GenreName = book.GenreName,
                            Price = book.Price,
                            QuantityInCart = item.Quantity
                        });
                    }
                    else
                    {
                        throw new UserException($"Book with ID {item.BookId} not found.");
                    }
                }

                return bookDetailsList;
            }



            // Cập nhật phương thức GetTotalPriceAsync
            public async Task<decimal> GetTotalPriceAsync(int userId)
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Book)  // Tải thông tin Book từ CartItem
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null) throw new UserException("Cart not found");

                decimal totalPrice = 0;
                foreach (var item in cart.CartItems)
                {
                    // Kiểm tra nếu giá trị sách hợp lệ
                    if (item.Book == null || item.Book.Price <= 0)
                    {
                        throw new UserException("Invalid book price.");
                    }

                    totalPrice += item.Quantity * item.Book.Price;
                }

                return totalPrice;
            }
        public async Task<string> ConfirmPurchaseAsync(int userId, bool confirm)
        {
            if (!confirm)
            {
                return "Bạn đã hủy thao tác mua hàng.";
            }

            // Lấy giỏ hàng của người dùng
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                throw new UserException("Giỏ hàng của bạn hiện không có sản phẩm.");
            }

            // Tính tổng giá trị giỏ hàng
            decimal totalAmount = 0;
            foreach (var cartItem in cart.CartItems)
            {
                var book = await _context.Books.FindAsync(cartItem.BookId);
                if (book == null) throw new UserException("Sản phẩm không tồn tại trong kho.");
                totalAmount += cartItem.Quantity * book.Price;
            }

            // Tạo đơn hàng mới
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Tạo các OrderDetails
            foreach (var item in cart.CartItems)
            {
                var book = await _context.Books.FindAsync(item.BookId);
                if (book == null) throw new UserException("Sản phẩm không tồn tại.");

                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = book.Price
                };

                _context.OrderDetails.Add(orderDetail);
            }

            // Lưu các OrderDetails
            await _context.SaveChangesAsync();

            // Xóa giỏ hàng sau khi tạo đơn hàng
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return "Đơn hàng của bạn đã được tạo thành công.";
        }
    }

}
    
