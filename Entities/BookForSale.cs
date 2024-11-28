using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShell.Entities
{
    public class BookForSale
    {
        [ForeignKey("Book")]
        public int BookId { get; set; }
        [ForeignKey("Inventory")]
        public int Quantity { get; set; }

        public Book? Book { get; set; }
        public Inventory? Inventory { get; set; }

    }
}
