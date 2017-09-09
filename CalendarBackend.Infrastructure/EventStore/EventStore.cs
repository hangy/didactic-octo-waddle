namespace CalendarBackend.Infrastructure.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class EventStore : IEventStore, IDisposable
    {

        private readonly EventReader reader;
        private readonly EventWriter writer;

        public EventStore(string path)
        {
            this.writer = new EventWriter(path);
            this.reader = new EventReader(path);
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
            return this.writer;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.writer?.Dispose();
                this.reader?.Dispose();
            }
        }
    }
}
