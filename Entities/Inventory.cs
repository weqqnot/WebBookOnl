using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShell.Entities
{
    public class Inventory
    {
        [Key]
        public int BookInventoryId { get; set; }
   
        public int BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CostPrice { get; set; } 
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SalePrice { get; set; }
        public int QuantityForSale { get; set; }
        public string? Title { get; set; }
        public int QuantityInStock { get; set; }

        public Book? Books { get; set; }
        public ICollection<BookForSale> BookForSale { get; set; }
    }
}