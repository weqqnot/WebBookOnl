using WebBookShell.DTO;
using WebBookShell.Entities;

namespace WebBookShell.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(int userId, CartRequest request);  // Thêm sản phẩm vào giỏ hàng
        Task UpdateQuantityAsync(int userId, int bookId, int quantity);  // Cập nhật số lượng sản phẩm
        Task RemoveFromCartAsync(int userId, int bookId);  // Xóa sản phẩm khỏi giỏ hàng
       
        Task<IEnumerable<BookDetails>> GetAllBooksDetailsInCartAsync(int userId);  // Lấy tất cả thông tin sách chi tiết trong giỏ hàng
        Task<decimal> GetTotalPriceAsync(int userId);  // Tính tổng giá trị giỏ hàng
        Task<string> ConfirmPurchaseAsync(int userId, bool confirm);
    }
}
