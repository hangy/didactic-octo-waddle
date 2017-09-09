namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEventReader : IDisposable
    {
        Task<IReadOnlyList<IDomainEvent>> ReadAllAsync(CancellationToken cancellationToken = default);
    }
}