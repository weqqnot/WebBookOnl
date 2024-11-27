using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShell.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? Users { get; set; }
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [ForeignKey("Voucher")]
        public int? VoucherId { get; set; }
        public Voucher Vouchers { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        
    }
}
