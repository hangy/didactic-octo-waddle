namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class EventReader : IEventReader
    {
        private readonly IReadOnlyList<IDomainEvent> list;

        private readonly SemaphoreSlim readWriteSemaphore;

        public EventReader(IReadOnlyList<IDomainEvent> list, SemaphoreSlim readWriteSemaphore)
        {
            this.list = list ?? throw new ArgumentNullException(nameof(list));
            this.readWriteSemaphore = readWriteSemaphore ?? throw new ArgumentNullException(nameof(readWriteSemaphore));
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public async Task<IReadOnlyList<IDomainEvent>> ReadAllAsync(CancellationToken cancellationToken = default)
        {
            await this.readWriteSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                return this.list;
            }
            finally
            {
                this.readWriteSemaphore.Release();
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
    }
}