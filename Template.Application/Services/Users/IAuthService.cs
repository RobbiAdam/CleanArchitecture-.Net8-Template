using Template.Contract.Authentications;

namespace Template.Application.Services.Users
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}
