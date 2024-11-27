using System.ComponentModel.DataAnnotations;

namespace WebBookShell.Entities
{
    public class Author
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string? AuthorName { get; set; } 

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
