using Televent.Core.Common.Interfaces;
using Televent.Core.Games.Models;

namespace Televent.Core.Games.Interfaces;

public interface IGameRepository : IRepository<Game, Guid>
{
    Task<Game?> GetLastGameAsync();
}