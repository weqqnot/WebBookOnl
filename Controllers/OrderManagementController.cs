using WebBookShell.DTO;
using WebBookShell.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebBookShell.Exception;

namespace WebBookShell.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Policy = "Admin")]  // Đảm bảo chỉ admin mới có quyền truy cập
    public class OrderManagementController : ControllerBase
    {
        private readonly IOrderManagementService _orderManagementService;

        public OrderManagementController(IOrderManagementService orderManagementService)
        {
            _orderManagementService = orderManagementService;
        }

        // Lấy tất cả đơn hàng
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderManagementService.GetAllOrdersAsync();
                if (orders == null || !orders.Any())
                {
                    return Ok(new { Message = "Không có đơn hàng nào trong hệ thống." });
                }
                return Ok(new { Message = "Danh sách đơn hàng:", Orders = orders });
            }
            catch (UserException ex)
            {
                return BadRequest(new { Message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }

        // Lấy tổng số đơn hàng
        [HttpGet("total-count")]
        public async Task<IActionResult> GetTotalOrdersCount()
        {
            try
            {
                var totalOrders = await _orderManagementService.GetTotalOrdersCountAsync();
                return Ok(new { Message = "Tổng số đơn hàng hiện có:", TotalOrders = totalOrders });
            }
            catch (UserException ex)
            {
                return BadRequest(new { Message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }

        // Lấy chi tiết một đơn hàng theo OrderId
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            try
            {
                var order = await _orderManagementService.GetOrderDetailsAsync(orderId);
                return Ok(new { Message = "Chi tiết đơn hàng:", Order = order });
            }
            catch (UserException ex)
            {
                return NotFound(new { Message = $"Đơn hàng không tồn tại: {ex.Message}" });
            }
        }
    }
}
