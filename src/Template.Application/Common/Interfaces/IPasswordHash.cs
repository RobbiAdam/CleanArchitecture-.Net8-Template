namespace Template.Application.Common.Interfaces
{
    public interface IPasswordHash
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
