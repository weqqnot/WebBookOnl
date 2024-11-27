namespace WebBookShell.DTO
{

    public class UserRegisterRequest
    {
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // Đăng nhập người dùng
    public class UserLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // Yêu cầu quên mật khẩu
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }

    // Cập nhật thông tin người dùng
    public class UpdateRequest
    {
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
