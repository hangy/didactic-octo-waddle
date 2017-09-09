namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEventStream
    {
        Task<IReadOnlyList<IDomainEvent>> ReadAllEventsAsync(CancellationToken cancellationToken = default);

        Task<int> WriteEventsAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
    }
}