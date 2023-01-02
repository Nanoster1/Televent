using Televent.Core.Common.Interfaces;
using Televent.Core.Events.Models;

namespace Televent.Core.Events.Interfaces;

public interface IEventRepository : IRepository<Event, int>
{

}