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

        public Book? Books { get; set; }
        
    }
}
