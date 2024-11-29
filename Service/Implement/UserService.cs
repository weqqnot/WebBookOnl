using WebBookShell.Entities;
using WebBookShell.DTO;
using WebBookShell.Exception;
using WebBookShell.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System;
using WebBookShell.Data;

namespace WebBookShell.Service
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;

        public UserService(ApplicationDbContext context, JwtHelper jwtHelper, IConfiguration configuration)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        public async Task<string> Register(UserRegisterRequest request)
        {
            // Kiểm tra nếu email đã tồn tại
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new UserException("Email đã tồn tại!");

            var passwordHash = PasswordHelper.HashPassword(request.Password);

            // Gán vai trò Admin cho người dùng đầu tiên
            var roleId = (await _context.Users.CountAsync()) == 0 ? 1 : 3; // Người dùng đầu tiên là Admin.

            var user = new User
            {
                Fullname = request.Fullname,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PasswordHash = passwordHash,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow,
        
            };

            // Thêm người dùng vào cơ sở dữ liệu
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Không tạo token ở đây, chỉ trả về thông báo hoặc mã xác thực
            return "Hãy đăng nhập để tạo token!";
        }


        public async Task<string> Login(UserLoginRequest request)
        {
            // Lấy thông tin người dùng từ database
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
                throw new UserException("Invalid credentials.");

            // Tạo token mới khi đăng nhập thành công
            var token = _jwtHelper.GenerateJwtToken(user.UserId, user.Email, user.Roles.RoleName);

  

            await _context.SaveChangesAsync();

            // Trả về token mới cho người dùng
            return token;
        }


        public async Task ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                throw new UserException("Email does not exist.");

            user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(int userId, UpdateRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                throw new UserException("User not found.");

            user.Fullname = request.Fullname;
            user.Address = request.Address;
            user.PhoneNumber = request.PhoneNumber;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                throw new UserException("User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
