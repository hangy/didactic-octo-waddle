namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using System;
    using System.Collections.Generic;
    using System.IO.Compression;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Linq;
    using Newtonsoft.Json;
    using System.IO;

    public class EventReader : IEventReader
    {
        private readonly SemaphoreSlim readWriteSemaphore;

        private readonly ZipArchive ziprchive;

        public EventReader(ZipArchive ziprchive, SemaphoreSlim readWriteSemaphore)
        {
            this.ziprchive = ziprchive ?? throw new ArgumentNullException(nameof(ziprchive));
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
                return this.ziprchive.Entries.OrderBy(e => e.FullName).Select(Transform).ToList();
            }
            finally
            {
                this.readWriteSemaphore.Release();
            }
        }

        /// <remarks>
        /// https://stackoverflow.com/a/17788118/11963
        /// </remarks>
        private static StoredEvent DeserializeFromStream(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<StoredEvent>(jsonTextReader);
            }
        }

        private static IDomainEvent Transform(ZipArchiveEntry arg)
        {
            using (var stream = arg.Open())
            {
                var storedEvent = DeserializeFromStream(stream);
                return (IDomainEvent)JsonConvert.DeserializeObject(storedEvent.Json, Type.GetType(storedEvent.TypeName));
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