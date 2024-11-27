using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShell.Entities
{
    public class Voucher
    {
        [Key]
        public int VoucherId { get; set; }
        [Required]
        public string? Code { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountAmount { get; set; }

        public DateTime? ExpireDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<BookVoucher> BookVouchers { get; set; } = new List<BookVoucher>();
       
    }
    public class BookVoucher
    {

        [ForeignKey("Book")]
        public int BookId { get; set; }

        [ForeignKey("Voucher")]
        public int VoucherId { get; set; }

        public Book? Books { get; set; }
        public Voucher? Vouchers { get; set; }
    }
}
