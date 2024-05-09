using Template.Application.Common.Interfaces;

namespace Template.Infrastructure.Security.PasswordHasher
{
    public class PasswordHash : IPasswordHash
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
