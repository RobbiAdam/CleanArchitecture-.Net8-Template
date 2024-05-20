using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces.Repositories;
using Template.Domain.Entities;
using Template.Infrastructure.Common.Persistence;

namespace Template.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        //public async Task<List<string>> GetUserRolesAsync(string userId)
        //{
        //    var user = await _context.Users.FindAsync(userId);

        //    if (user == null)
        //    {
        //        return new List<string>();
        //    }

        //    var roles = new List<string>();

        //    if (user.isAdmin)
        //    {
        //        roles.Add("admin");
        //    }
        //    else
        //    {
        //        roles.Add("user");
        //    }

        //    return roles;
        //}

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
