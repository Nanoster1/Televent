namespace Televent.Core.Common.Interfaces;

public interface IUnitOfWork
{
    Task SaveAsync();
}