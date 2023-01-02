using Microsoft.Extensions.Caching.Memory;
using Televent.Core.Common.Interfaces;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;

namespace Televent.Core.Users.Services;

public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserManager(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMemoryCache memoryCache)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(User entity)
    {
        await _userRepository.InsertAsync(entity);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(User entity)
    {
        await _userRepository.DeleteAsync(entity);
        await _unitOfWork.SaveAsync();
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public IAsyncEnumerable<User> ListAllAsync()
    {
        return _userRepository.ListAllAsync();
    }

    public async Task UpdateAsync(User entity)
    {
        await _userRepository.UpdateAsync(entity);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(IEnumerable<User> entities)
    {
        foreach (var entity in entities)
        {
            await _userRepository.UpdateAsync(entity);
        }
        await _unitOfWork.SaveAsync();
    }
}