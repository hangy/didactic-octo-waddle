namespace CalendarBackend.Infrastructure.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using CalendarBackend.Domain.Events;
    using Newtonsoft.Json;

    public class EventWriter : IEventWriter
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        private readonly JsonSerializer jsonSerializer;

        private readonly JsonWriter jsonWriter;

        public EventWriter(JsonSerializer jsonSerializer, string path)
        {
            this.jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            this.jsonWriter = new JsonTextWriter(new StreamWriter(new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, true)));
        }

        public async Task<IReadOnlyList<IDomainEvent>> AppendAllAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
        {
            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            await this.semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                // this.jsonSerializer.Serialize()
                throw new NotImplementedException();
            }
            finally
            {
                this.semaphore.Release();
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
                this.semaphore?.Dispose();
                (this.jsonWriter as IDisposable)?.Dispose();
            }
        }
    }
}