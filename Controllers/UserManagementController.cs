using Microsoft.AspNetCore.Mvc;
using WebBookShell.DTO;
using WebBookShell.Service;
using WebBookShell.Exception;
using Microsoft.AspNetCore.Authorization;
using WebBookShell.Helper;
using WebBookShell.Service.Interface;

namespace WebBookShell.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly JwtHelper _jwtHelper; // Chỉ sử dụng JwtHelper

        public UserManagementController(IUserManagementService userManagementService, JwtHelper jwtHelper)
        {
            _userManagementService = userManagementService;
            _jwtHelper = jwtHelper; // Inject JwtHelper thay vì ITokenService
        }

        // Lấy thông tin người dùng theo email
        [HttpGet("{email}")]
        public async Task<ActionResult<UserDTO>> GetUserByEmail(string email)
        {
            // Lấy token từ header Authorization
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Log ra token nhận được trong header
            Console.WriteLine($"Received Token: {token}");

            try
            {
                // Kiểm tra tính hợp lệ của token và lấy role từ token
                var isValidToken = _jwtHelper.ValidateToken(token); // Sử dụng JwtHelper để xác thực token
                if (!isValidToken)
                {
                    return Unauthorized(new { message = "Invalid or expired token" });
                }

                // Lấy role từ token
                var role = _jwtHelper.GetRoleFromToken(token);
                if (role != "Admin")
                {
                    return Unauthorized(new { message = "You do not have permission to access this resource" });
                }

                // Lấy thông tin người dùng từ database
                var user = await _userManagementService.GetUserByEmailAsync(email);
                return Ok(user);
            }
            catch (UserException ex)
            {
                return NotFound(new { message = ex.Message }); // Nếu không tìm thấy người dùng
            }
        }

        // Thay đổi role người dùng theo email
        [HttpPut("{email}/role")]
        public async Task<IActionResult> ChangeUserRole(string email, [FromBody] RoleChangeRequest request)
        {
            // Lấy token từ header Authorization
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Log ra token nhận được trong header
            Console.WriteLine($"Received Token: {token}");

            try
            {
                // Kiểm tra tính hợp lệ của token và lấy role từ token
                var isValidToken = _jwtHelper.ValidateToken(token); // Sử dụng JwtHelper để xác thực token
                if (!isValidToken)
                {
                    return Unauthorized(new { message = "Invalid or expired token" });
                }

                // Lấy role từ token
                var role = _jwtHelper.GetRoleFromToken(token);
                if (role != "Admin")
                {
                    return Unauthorized(new { message = "You do not have permission to access this resource" });
                }

                // Thay đổi role cho người dùng theo email
                var user = await _userManagementService.ChangeUserRoleAsync(email, request.NewRole);

                // Tạo lại token mới sau khi thay đổi role
                var newToken = _jwtHelper.GenerateJwtToken(user.UserId, user.Email, user.Role);

                // Trả về token mới
                return Ok(new { message = "Role người dùng đã được cập nhật thành công", token = newToken });
            }
            catch (UserException ex)
            {
                return BadRequest(new { message = ex.Message }); // Xử lý lỗi khi thay đổi role
            }
        }
    }
}
