using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBookShell.Entities;

public class CartItem
{
    [Key]
    public int CartItemId { get; set; }

    [ForeignKey("Cart")]
    public int CartId { get; set; }

    [ForeignKey("Book")]
    public int BookId { get; set; }

    [Required]
    public int Quantity { get; set; }

    public Cart? Cart { get; set; }  // Đổi từ Carts thành Cart
    public Book? Book { get; set; }  // Đổi từ Books thành Book
}
