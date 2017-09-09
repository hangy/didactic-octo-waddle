namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEventWriter : IDisposable
    {
        Task<IReadOnlyList<IDomainEvent>> AppendAllAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
    }
}