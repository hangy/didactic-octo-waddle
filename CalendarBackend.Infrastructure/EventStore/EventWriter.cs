namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class EventWriter : IEventWriter
    {
        private readonly IList<IDomainEvent> list;

        private readonly SemaphoreSlim readWriteSemaphore;

        public EventWriter(IList<IDomainEvent> list, SemaphoreSlim readWriteSemaphore)
        {
            this.list = list ?? throw new ArgumentNullException(nameof(list));
            this.readWriteSemaphore = readWriteSemaphore ?? throw new ArgumentNullException(nameof(readWriteSemaphore));
        }

        public async Task<IReadOnlyList<IDomainEvent>> AppendAllAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
        {
            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            await this.readWriteSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                var result = new List<IDomainEvent>();
                foreach (var @event in events)
                {
                    this.list.Add(@event);
                    result.Add(@event);
                }

                return result;
            }
            finally
            {
                this.readWriteSemaphore.Release();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}