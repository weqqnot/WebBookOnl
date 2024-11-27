using System.Security.Cryptography;
using System.Text;

namespace WebBookShell.Helper
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // Cách mã hóa mật khẩu
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }
    }

}
