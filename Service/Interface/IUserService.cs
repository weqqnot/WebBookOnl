using WebBookShell.DTO;

namespace WebBookShell.Service
{
    public interface IUserService
    {
       
        Task<string> Register(UserRegisterRequest request);
        Task<string> Login(UserLoginRequest request);
        Task ForgotPassword(ForgotPasswordRequest request);
        Task UpdateUser(int userId, UpdateRequest request);
        Task DeleteUser(int userId);
    }
}
