using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBookShell.Entities
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int RoleId { get; set; } 

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } // Tên role (Admin, Staff, Customer)

        // Quan hệ 1-Nhiều với User
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
