namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Threading;
    using System.Threading.Tasks;

    public class EventWriter : IEventWriter
    {
        private readonly SemaphoreSlim readWriteSemaphore;

        private readonly ZipArchive zipArchive;

        public EventWriter(ZipArchive zipArchive, SemaphoreSlim readWriteSemaphore)
        {
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
                    WriteEntry(@event, entry);
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

        /// <remarks>
        /// https://stackoverflow.com/a/17788118/11963
        /// </remarks>
        private static void SerializeToStream(Stream stream, StoredEvent storedEvent)
        {
            var serializer = new JsonSerializer();

            using (var sw = new StreamWriter(stream))
            using (var jsonTextWriter = new JsonTextWriter(sw))
            {
                serializer.Serialize(jsonTextWriter, storedEvent);
            }
        }

        private static StoredEvent Transform(IDomainEvent @event) => new StoredEvent(@event.GetType().AssemblyQualifiedName, JsonConvert.SerializeObject(@event));

        private static void WriteEntry(IDomainEvent @event, ZipArchiveEntry entry)
        {
            using (var stream = entry.Open())
            {
                SerializeToStream(stream, Transform(@event));
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