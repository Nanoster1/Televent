using Televent.Core.Users.Models;

namespace Televent.Core.Users.Interfaces;

public interface IUserManager
{
    Task<User?> GetByIdAsync(long id);
    IAsyncEnumerable<User> ListAllAsync();
    Task AddAsync(User entity);
    Task UpdateAsync(User entity);
    Task DeleteAsync(User entity);
}