using System.ComponentModel.DataAnnotations;

namespace WebBookShell.Entities
{
    public class BookGenre
    {

        public int BookId { get; set; }
        public Book? Books { get; set; }

        public string? GenreName { get; set; }
        public Genre? Genres { get; set; }


    }
}
