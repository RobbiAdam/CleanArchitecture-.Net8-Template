using Template.Domain.Entities;

namespace Template.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        //Task<List<String>> GetUserRolesAsync(string id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
