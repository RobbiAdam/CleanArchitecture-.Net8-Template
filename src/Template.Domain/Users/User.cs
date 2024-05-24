using Template.Domain.Abstractions;

namespace Template.Domain.Users
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsAdmin { get; set; } = false;
    }
}
