using System.Threading.Tasks;


namespace App1.Data
{
    public class AuthService : IAuthService
    {
        public bool IsAuthenticated => throw new System.NotImplementedException();

        public Task<AuthResult> Authenticate(string id, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<ForgotPasswordResult> ForgotPassword(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResetPasswordResult> ResetPassword(string token, string newPassword)
        {
            throw new System.NotImplementedException();
        }
    }
}
