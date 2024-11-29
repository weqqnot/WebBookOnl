using System.ComponentModel.DataAnnotations;

namespace WebBookShell.Entities
{
    public class Genre
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string? GenreName { get; set; } 
        public string? GenreDescription { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    }
}
