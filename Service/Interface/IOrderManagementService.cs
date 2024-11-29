using WebBookShell.DTO;

namespace WebBookShell.Interfaces
{
    public interface IOrderManagementService
    {
        // Lấy tất cả đơn hàng
        Task<IEnumerable<AllOrdersDto>> GetAllOrdersAsync();

        // Lấy tổng số lượng đơn hàng
        Task<int> GetTotalOrdersCountAsync();

        // Lấy chi tiết một đơn hàng
        Task<OrderDto> GetOrderDetailsAsync(int orderId);
    }
}
