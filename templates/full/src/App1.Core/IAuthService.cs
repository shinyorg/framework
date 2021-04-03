using System;
using System.Threading.Tasks;


namespace App1
{
    public interface IAuthService
    {
        bool IsAuthenticated { get; }

        Task<AuthResult> Authenticate(string id, string password);
        Task<ForgotPasswordResult> ForgotPassword(string id);
        Task<ResetPasswordResult> ResetPassword(string token, string newPassword);
    }
}
