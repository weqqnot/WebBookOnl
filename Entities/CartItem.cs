using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShell.Entities
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        [ForeignKey("Book")]    
        public int BookId { get; set; }
        [Required]
        public string Quantity { get; set; }

        public Cart? Carts { get; set; }
        public Book? Books { get; set; }
    }
}
