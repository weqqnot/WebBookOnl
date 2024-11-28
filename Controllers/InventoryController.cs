using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("Add")]
        public async Task<IActionResult> AddBook([FromBody] DTO.InventoryRequest.AddBookRequest request)
        {
            await _inventoryService.AddBookToInven(request.BookId, request.Quantity);
            return Ok("Book added to inventory.");
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateQuantity([FromBody] DTO.InventoryRequest.UpdateQuantityRequest request)
        {
            await _inventoryService.UpdateBookQuantity(request.BookId, request.Quantity);
            return Ok("Book quantity updated.");
        }

        [HttpGet("Quantity/{BookId}")]
        public async Task<IActionResult> GetQuantity(int BookId)
        {
            var quantity = await _inventoryService.GetBookQuantity(BookId);
            return Ok(quantity);
        }

        [HttpDelete("Delete/{BookId}")]
        public async Task<IActionResult> RemoveBook(int BookId)
        {
            await _inventoryService.DeleteBookFromInven(BookId);
            return Ok("Book removed from inventory.");
        }
    }
}
