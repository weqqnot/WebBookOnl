using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebBookShell.DTO;
using WebBookShell.DTOs;
using WebBookShell.Entities;
using WebBookShell.Service;
using WebBookShell.Service.Interface;

namespace WebBookShell.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // Thêm sách vào kho
        [HttpPost("add")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddBook([FromBody] Inventory inventory)
        {
            try
            {
                var addedBook = await _inventoryService.AddBookAsync(inventory);
                return CreatedAtAction(nameof(AddBook), new { id = addedBook.InventoryBookId }, new { message = "Sách đã được thêm vào kho thành công", addedBook });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Cập nhật thông tin sách
        [HttpPut("update/{InventoryBookId}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateBook(int InventoryBookId, [FromBody] Inventory updatedInventory, [FromQuery] int quantityChange)
        {
            try
            {
                var updatedBook = await _inventoryService.UpdateBookAsync(InventoryBookId, updatedInventory, quantityChange);
                return Ok(new { message = "Thông tin sách đã được cập nhật thành công", updatedBook });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Sách không tồn tại" });
            }
        }

        // Xóa sách khỏi kho
        [HttpDelete("{InventoryBookId}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteBook(int InventoryBookId)
        {
            try
            {
                await _inventoryService.DelteBookAsync(InventoryBookId);
                return NoContent(); // HTTP 204 No Content
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Sách không tồn tại" });
            }
        }

        // Lấy thông tin sách trong kho
        [HttpGet("{InventoryBookId}/Inventory")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<Inventory>> GetInventory(int InventoryBookId)
        {
            try
            {
                var inventory = await _inventoryService.GetInventoryByBookIdAsync(InventoryBookId);
                return Ok(new { message = "Thông tin kho sách đã được tải thành công", inventory });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy thông tin kho cho sách này" });
            }
        }

        // Xem toàn bộ sách trong kho
        [HttpGet("GetAllInventory")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<List<Inventory>>> GetAllInventory()
        {
            try
            {
                var inventoryList = await _inventoryService.GetAllInventoryAsync();
                return Ok(new { message = "Danh sách sách trong kho đã được tải thành công", inventoryList });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không có sách nào trong kho" });
            }
        }
    }
}
