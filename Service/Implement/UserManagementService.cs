using WebBookShell.Service.Interface;
using WebBookShell.DTO;
using WebBookShell.Entities;
using WebBookShell.Exception;
using Microsoft.EntityFrameworkCore;
using WebBookShell.Data;

namespace WebBookShell.Service
{
    public class UserManagementService : IUserManagementService
    {
        private readonly ApplicationDbContext _context;

        public UserManagementService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy thông tin người dùng theo email
        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .Include(u => u.Roles) // Bao gồm thông tin Role liên quan
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UserException("Người dùng không tồn tại với email này");
            }

            return new UserDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Role = user.Roles.RoleName,// Lấy tên role từ bảng Role
                Fullname = user.Fullname,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
            };
        }

        // Thay đổi role người dùng theo email
        public async Task<UserDTO> ChangeUserRoleAsync(string email, string newRole)
        {
            var user = await _context.Users
                .Include(u => u.Roles) // Bao gồm thông tin Role liên quan
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UserException("Người dùng không tồn tại với email này");
            }

            // Kiểm tra role có hợp lệ không
            var role = await _context.Roles
                .Where(r => r.RoleName == newRole)
                .FirstOrDefaultAsync();

            if (role == null)
            {
                throw new UserException("Role không hợp lệ");
            }

            // Cập nhật role mới cho người dùng
            user.RoleId = role.RoleId;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Role = role.RoleName // Trả về tên role mới
            };
        }
    }
}
