using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebBookShell.Entities
{
    public class Inventory
    {
        [Key]
        public int InventoryBookId { get; set; }
        public string Title { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CostPrice { get; set; } // giá nhập vào
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

    }
}
