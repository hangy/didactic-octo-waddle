namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class EventWriter : IEventWriter
    {
        private readonly JsonSerializer jsonSerializer;

        private readonly SemaphoreSlim readWriteSemaphore;

        private readonly ZipArchive zipArchive;

        public EventWriter(JsonSerializer jsonSerializer, ZipArchive zipArchive, SemaphoreSlim readWriteSemaphore)
        {
            this.jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            this.zipArchive = zipArchive ?? throw new ArgumentNullException(nameof(zipArchive));
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
                var count = this.zipArchive.Entries.Count;

                foreach (var @event in events)
                {
                    var entry = this.zipArchive.CreateEntry($"{++count:D10}.json", CompressionLevel.Optimal);
                    this.WriteEntry(@event, entry);
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

        /// <remarks>
        /// https://stackoverflow.com/a/17788118/11963
        /// </remarks>
        private void SerializeToStream(Stream stream, StoredEvent storedEvent)
        {
            using (var sw = new StreamWriter(stream))
            using (var jsonTextWriter = new JsonTextWriter(sw))
            {
                this.jsonSerializer.Serialize(jsonTextWriter, storedEvent);
            }
        }

        private string SerializeToString(IDomainEvent @event)
        {
            var sb = new StringBuilder(128);
            var sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = this.jsonSerializer.Formatting;
                this.jsonSerializer.Serialize(jsonWriter, @event, null);
            }

            return sw.ToString();
        }

        private StoredEvent Transform(IDomainEvent @event) => new StoredEvent(@event.GetType().AssemblyQualifiedName, this.SerializeToString(@event));

        private void WriteEntry(IDomainEvent @event, ZipArchiveEntry entry)
        {
            using (var stream = entry.Open())
            {
                this.SerializeToStream(stream, this.Transform(@event));
            }
        }
    }
}