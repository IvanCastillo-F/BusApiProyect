using BusApiProyect.Data.Models;

namespace BusApiProyect.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsyc();
        Task<User> GetUsersByIdAsyc(int id);
        Task UpdateUserAsync(User user);
    }
}