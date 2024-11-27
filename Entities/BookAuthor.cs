using System.ComponentModel.DataAnnotations;

namespace WebBookShell.Entities
{
    public class BookAuthor
    {

        public string? AuthorName { get; set; }
        public int BookId { get; set; }

        // Navigation properties
        public Author? Author { get; set; }
        public Book? Book { get; set; }

    }
}
