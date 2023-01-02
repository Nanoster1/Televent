using Microsoft.EntityFrameworkCore;
using Televent.Core.Games.Interfaces;
using Televent.Core.Games.Models;
using Televent.Data.Common.Shared;

namespace Televent.Data.Repositories;

public class GameRepository : RepositoryBase<Game, Guid>, IGameRepository
{
    public GameRepository(TeleventContext context) : base(context)
    {
    }

    public async Task<Game?> GetLastGameAsync()
    {
        return await Set.OrderByDescending(x => x.StartTime).FirstOrDefaultAsync();
    }
}