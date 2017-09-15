namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class EventStore : IEventStore, IDisposable
    {

        private readonly IEventReader reader;
        private readonly SemaphoreSlim readWriteSemaphore = new SemaphoreSlim(1);

        private readonly IEventWriter writer;

        public EventStore()
        {
            var sharedList = new List<IDomainEvent>();
            this.writer = new EventWriter(sharedList, readWriteSemaphore);
            this.reader = new EventReader(sharedList, readWriteSemaphore);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<IEventReader> GetReaderAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.reader);
        }

        public Task<IEventWriter> GetWriterAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.writer);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.readWriteSemaphore?.Dispose();
                this.writer?.Dispose();
                this.reader?.Dispose();
            }
        }
    }
}
