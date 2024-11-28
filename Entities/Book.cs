using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShell.Entities
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }
        [Required]
        [ForeignKey("Author")]
        public string? AuthorName { get; set; }
        

        [Required]
        [ForeignKey("Genre")]
        public string? GenreName { get; set; }
        public Genre? Genres { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }


        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
        public ICollection<BookVoucher> BookVouchers { get; set; } = new List<BookVoucher>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<Author> Authors { get; set; } = new List<Author>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
        public ICollection<BookForSale> BookForSale { get; set; }



    }
}
