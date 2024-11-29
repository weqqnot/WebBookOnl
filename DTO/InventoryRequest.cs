using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShell.DTO
{
    public class InventoryRequest
    {
        
            public int InventoryBookId { get; set; }
            public int Quantity { get; set; }
            public string? Title { get; set; }

            public string? Description { get; set; }

            [Required]
            public string? AuthorName { get; set; }

            [Required]
            public string? GenreName { get; set; }

            [Required]
            public decimal CostPrice { get; set; }
            public int QuantityInStock { get; set; }
        

    }
}
