namespace CalendarBackend.Infrastructure.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using CalendarBackend.Domain.Events;
    using Newtonsoft.Json;

    public class EventReader : IEventReader
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        private readonly JsonSerializer jsonSerializer;

        private readonly JsonReader jsonReader;

        public EventReader(JsonSerializer jsonSerializer, string path)
        {
            this.jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            this.jsonReader = new JsonTextReader(new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, true)));
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public async Task<IReadOnlyList<IDomainEvent>> ReadAllAsync(CancellationToken cancellationToken = default)
        {
            await this.semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                this.jsonSerializer.Deserialize(this.jsonReader);
            }
            finally
            {
                this.semaphore.Release();
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.semaphore?.Dispose();
                (this.jsonReader as IDisposable)?.Dispose();
            }
        }
    }
}