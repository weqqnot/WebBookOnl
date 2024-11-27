using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBookShell.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Role? Roles { get; set; }
        public Cart? Carts { get; set; }
        public ICollection<MembershipCard> MembershipCards { get; set; } = new List<MembershipCard>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
