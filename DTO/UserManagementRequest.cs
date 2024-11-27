using System.ComponentModel.DataAnnotations;

namespace WebBookShell.DTO
{
    public class RoleChangeRequest
    {
        [Required]
        public string NewRole { get; set; }
    }
    public class UserDTO
    {
        public int UserId { get; set; } 
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; } 
        public string Role { get; set; } 
    }
}
