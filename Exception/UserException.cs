using System;

namespace WebBookShell.Exception
{
    public class UserException : IOException
    {
        // Constructor nhận một thông báo lỗi
        public UserException(string message) : base(message)
        {

        }


    }
}
