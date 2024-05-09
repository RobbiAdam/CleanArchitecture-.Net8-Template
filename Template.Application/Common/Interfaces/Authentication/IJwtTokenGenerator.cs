namespace Template.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string userId, string userName, string email, string role);
    }
}
