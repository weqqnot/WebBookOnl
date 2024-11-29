using Microsoft.AspNetCore.Mvc;
using WebBookShell.DTO;
using WebBookShell.Service;
using WebBookShell.Exception;

namespace WebBookShell.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Đăng ký người dùng
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            try
            {
                var token = await _userService.Register(request);
                return Ok(new { message = "Đăng ký thành công", Token = token });
            }
            catch (UserException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Đăng nhập người dùng
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            try
            {
                var token = await _userService.Login(request);
                return Ok(new { message = "Đăng nhập thành công", Token = token });
            }
            catch (UserException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Quên mật khẩu
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                await _userService.ForgotPassword(request);
                return Ok(new { message = "Mật khẩu đã được cập nhật thành công." });
            }
            catch (UserException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Cập nhật thông tin người dùng
        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateRequest request)
        {
            try
            {
                await _userService.UpdateUser(userId, request);
                return Ok(new { message = "Thông tin người dùng đã được cập nhật thành công." });
            }
            catch (UserException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Xóa người dùng
        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                await _userService.DeleteUser(userId);
                return Ok(new { message = "Người dùng đã được xóa thành công." });
            }
            catch (UserException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
