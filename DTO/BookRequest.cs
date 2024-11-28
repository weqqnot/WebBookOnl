using System.ComponentModel.DataAnnotations;

namespace WebBookShell.DTOs
{
    public class BookRequest 
    {
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public string AuthorName { get; set; }

        [Required]
        public string GenreName { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
