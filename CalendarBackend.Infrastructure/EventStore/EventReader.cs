namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class EventReader : IEventReader
    {
        private readonly JsonSerializer jsonSerializer;

        private readonly SemaphoreSlim readWriteSemaphore;

        private readonly string path;

        public EventReader(JsonSerializer jsonSerializer, string path, SemaphoreSlim readWriteSemaphore)
        {
            this.jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            this.path = path ?? throw new ArgumentNullException(nameof(path));
            this.readWriteSemaphore = readWriteSemaphore ?? throw new ArgumentNullException(nameof(readWriteSemaphore));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<IReadOnlyList<IDomainEvent>> ReadAllAsync(CancellationToken cancellationToken = default)
        {
            await this.readWriteSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                using var zipArchive = ZipFile.Open(this.path, ZipArchiveMode.Read);
                return zipArchive.Entries.OrderBy(e => e.FullName).Select(this.Transform).Where(e => e != null).Cast<IDomainEvent>().ToImmutableList();
            }
            finally
            {
                this.readWriteSemaphore.Release();
            }
        }

        /// <remarks>
        /// https://stackoverflow.com/a/17788118/11963
        /// </remarks>
        private StoredEvent? DeserializeFromStream(Stream stream)
        {
            using var sr = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(sr);
            return this.jsonSerializer.Deserialize<StoredEvent?>(jsonTextReader);
        }

        private IDomainEvent? DeserializeFromString(string value, Type type)
        {
            var sr = new StringReader(value);
            using var jsonReader = new JsonTextReader(sr);
            return this.jsonSerializer.Deserialize(jsonReader, type) as IDomainEvent;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        private IDomainEvent? Transform(ZipArchiveEntry arg)
        {
            using var stream = arg.Open();
            var storedEvent = this.DeserializeFromStream(stream);
            if (storedEvent is null)
            {
                return null;
            }

            var type = Type.GetType(storedEvent.TypeName);
            if (type is null)
            {
                return null;
            }

            return this.DeserializeFromString(storedEvent.Json, type);
        }
    }
}