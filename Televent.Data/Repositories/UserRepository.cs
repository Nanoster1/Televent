using Televent.Core.Users.Interfaces;
using Televent.Core.Users.Models;
using Televent.Data.Common.Shared;

namespace Televent.Data.Repositories;

public class UserRepository : RepositoryBase<User, long>, IUserRepository
{
    public UserRepository(TeleventContext context) : base(context)
    {
    }
}