namespace CalendarBackend.Infrastructure.EventStore
{
    using Newtonsoft.Json;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class EventStore : IEventStore, IDisposable
    {
        private readonly IEventReader reader;

        private readonly SemaphoreSlim readWriteSemaphore = new SemaphoreSlim(1);

        private readonly IEventWriter writer;

        public EventStore(JsonSerializer jsonSerializer, string path)
        {
            if (jsonSerializer == null)
            {
                throw new ArgumentNullException(nameof(jsonSerializer));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.writer = new EventWriter(jsonSerializer, path, readWriteSemaphore);
            this.reader = new EventReader(jsonSerializer, path, readWriteSemaphore);
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
