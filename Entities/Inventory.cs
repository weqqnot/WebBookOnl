using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShell.Entities
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CostPrice { get; set; } // giá nhập vào
        public int QuantityForSale { get; set; }

        public Book? Books { get; set; }
        public ICollection<BookForSale> BookForSale { get; set; }
    }
}
