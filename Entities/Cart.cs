using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShell.Entities
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [Required]        
        
        [ForeignKey ("User") ]
        public int UserId { get; set; }
        public User? Users { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
