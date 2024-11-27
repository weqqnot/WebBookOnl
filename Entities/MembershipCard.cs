using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShell.Entities
{
    public class MembershipCard
    {
        [Key]
        public int MembershipCardId { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }
        [Required]
        public string? CardType { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountRate { get; set; }

        public User? Users { get; set; }
    }
}
