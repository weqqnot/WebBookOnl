using WebBookShell.DTO;
using WebBookShell.Entities;
using WebBookShell.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebBookShell.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartRequest request)
        {
            var userId = GetUserIdFromToken();
            await _cartService.AddToCartAsync(userId, request);
            return Ok(new { Message = "Sản phẩm đã được thêm vào giỏ hàng thành công." });
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuantity([FromBody] CartRequest request)
        {
            var userId = GetUserIdFromToken();
            await _cartService.UpdateQuantityAsync(userId, request.BookId, request.Quantity);
            return Ok(new { Message = "Số lượng sản phẩm trong giỏ hàng đã được cập nhật thành công." });
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpDelete("remove/{bookId}")]
        public async Task<IActionResult> RemoveFromCart(int bookId)
        {
            var userId = GetUserIdFromToken();
            await _cartService.RemoveFromCartAsync(userId, bookId);
            return Ok(new { Message = "Sản phẩm đã được xóa khỏi giỏ hàng thành công." });
        }

        // Lấy tất cả thông tin chi tiết sách trong giỏ hàng của người dùng
        [HttpGet("details")]
        public async Task<IActionResult> GetAllBooksDetailsInCart()
        {
            var userId = GetUserIdFromToken();
            var bookDetails = await _cartService.GetAllBooksDetailsInCartAsync(userId);
            if (bookDetails == null || !bookDetails.Any())
            {
                return Ok(new { Message = "Giỏ hàng của bạn hiện tại không có sản phẩm." });
            }
            return Ok(new { Message = "Danh sách sách trong giỏ hàng của bạn:", Books = bookDetails });
        }

        // Lấy tổng giá trị giỏ hàng
        [HttpGet("total-price")]
        public async Task<IActionResult> GetTotalPrice()
        {
            var userId = GetUserIdFromToken();
            var totalPrice = await _cartService.GetTotalPriceAsync(userId);
            return Ok(new { Message = "Tổng giá trị giỏ hàng của bạn là:", TotalPrice = totalPrice });
        }
        [HttpPost("confirm-purchase")]
        public async Task<IActionResult> ConfirmPurchase([FromBody] bool confirm)
        {
            var userId = GetUserIdFromToken();
            var message = await _cartService.ConfirmPurchaseAsync(userId, confirm);
            return Ok(new { Message = message });
        }

        // Hàm lấy userId từ JWT token
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
            {
                throw new UnauthorizedAccessException("Không tìm thấy UserId trong token.");
            }
            return int.Parse(userIdClaim.Value);
        }
    }
}
