namespace WebBookShell.DTO
{
    // DTO cho thông tin chi tiết một đơn hàng
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } // Tên người dùng (lấy từ Users)
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }

    // DTO cho chi tiết sản phẩm trong đơn hàng
    public class OrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; } // Tên sách
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Giá sách
    }

    // DTO cho tổng số đơn hàng
    public class TotalOrdersDto
    {
        public int TotalOrders { get; set; }
    }

    // DTO cho thông tin đơn hàng cơ bản (dành cho danh sách đơn hàng)
    public class AllOrdersDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } // Tên người dùng
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
