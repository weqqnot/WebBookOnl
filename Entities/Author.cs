using System.ComponentModel.DataAnnotations;

namespace WebBookShell.Entities
{
    public class Author
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string? AuthorName { get; set; } 
        public string? AuthorDescription { get; set; }


        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    }
}
