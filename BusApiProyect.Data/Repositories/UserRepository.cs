using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace BusApiProyect.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _context;
        public UserRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsyc()
        {
            var users = await _context.Users.ToArrayAsync();
            return users;
        }

        public async Task<User> GetUsersByIdAsyc(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUsersByCredentialsAsyc(string email, string password)
        {
            // Retrieve the user with the provided email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                // Return null if the user with the provided email does not exist
                return null;
            }

            // Return the user if credentials are valid
            return user;
        }

        public async Task<bool> IsEmailDuplicateAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsEmailDuplicateForOtherUserAsync(string email, int userId)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.Id != userId);
        }


    }
}
