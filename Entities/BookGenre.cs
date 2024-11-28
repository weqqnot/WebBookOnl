using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebBookShell.Entities
{
    public class BookGenre
    {

        public int BookId { get; set; }

        [JsonIgnore]
        public Book? Books { get; set; }

        public string? GenreName { get; set; }
        [JsonIgnore]
        public Genre? Genres { get; set; }


    }
}
