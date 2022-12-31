using Microsoft.Extensions.Caching.Memory;
using Televent.Core.Common.Interfaces;
using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;

namespace Televent.Core.Users.Services;

public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly IUnitOfWork _unitOfWork;

    public UserManager(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMemoryCache memoryCache)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
    }

    public async Task AddAsync(User entity)
    {
        await _userRepository.InsertAsync(entity);
        await _unitOfWork.SaveAsync();
        _memoryCache.Set(entity.Id, entity);
    }

    public async Task DeleteAsync(User entity)
    {
        await _userRepository.DeleteAsync(entity);
        await _unitOfWork.SaveAsync();
        _memoryCache.Remove(entity.Id);
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        if (_memoryCache.TryGetValue(id, out User? user)) return user;
        user = await _userRepository.GetByIdAsync(id);
        if (user is not null) _memoryCache.Set(id, user);
        return user;
    }

    public IAsyncEnumerable<User> ListAllAsync()
    {
        return _userRepository.ListAllAsync();
    }

    public async Task UpdateAsync(User entity)
    {
        await _userRepository.UpdateAsync(entity);
        await _unitOfWork.SaveAsync();
        _memoryCache.Set(entity.Id, entity);
    }
}