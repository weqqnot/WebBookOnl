using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Genre? Genres { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }




        [JsonIgnore]
        public ICollection<BookVoucher> BookVouchers { get; set; } = new List<BookVoucher>();
        [JsonIgnore]
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        [JsonIgnore]
        public ICollection<Author> Authors { get; set; } = new List<Author>();
        [JsonIgnore]
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        [JsonIgnore]
        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
       




    }
}
