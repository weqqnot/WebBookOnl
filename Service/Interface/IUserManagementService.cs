using WebBookShell.DTO;

namespace WebBookShell.Service.Interface
{
    public interface IUserManagementService
    {
        // Lấy thông tin người dùng theo email
        Task<UserDTO> GetUserByEmailAsync(string email);

        // Thay đổi role người dùng theo email
        Task<UserDTO> ChangeUserRoleAsync(string email, string newRole);
    }
}
