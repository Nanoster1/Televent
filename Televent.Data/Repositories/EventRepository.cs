using Microsoft.EntityFrameworkCore;
using Televent.Core.Events.Interfaces;
using Televent.Core.Events.Models;
using Televent.Data.Common.Shared;

namespace Televent.Data.Repositories;

public class EventRepository : RepositoryBase<Event, int>, IEventRepository
{
    public EventRepository(TeleventContext context) : base(context)
    {
    }

    public async Task<IList<Event>> ListAllNotExecutedAsync()
    {
        return await Set.AsNoTracking()
            .Where(e => !e.IsExecuted)
            .OrderBy(e => e.ExecutionTime)
            .ToListAsync();
    }
}