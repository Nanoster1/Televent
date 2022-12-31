using Televent.Core.Common.Interfaces;

namespace Televent.Data.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly TeleventContext _context;

    public UnitOfWork(TeleventContext context)
    {
        _context = context;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}