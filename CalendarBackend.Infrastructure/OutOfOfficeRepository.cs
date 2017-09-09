namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class OutOfOfficeRepository : IOutOfOfficeRepository
    {
        private readonly Func<Task<IEventStoreConnection>> connectionFactory;

        private readonly JsonSerializer jsonSerializer;

        public OutOfOfficeRepository(Func<Task<IEventStoreConnection>> connectionFactory, JsonSerializer jsonSerializer)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this.jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        public async Task<OutOfOffice> AddAsync(OutOfOffice outOfOffice, CancellationToken cancellationToken = default)
        {
            if (outOfOffice == null)
            {
                throw new ArgumentNullException(nameof(outOfOffice));
            }

            var eventData = new List<EventData>();
            foreach (var @event in outOfOffice.DomainEvents)
            {
                using (var stringWriter = new StringWriter())
                {
                    this.jsonSerializer.Serialize(stringWriter, @event);

                    eventData.Add(new EventData(
                             @event.Id,
                             @event.GetType().Name,
                             true,
                             Encoding.UTF8.GetBytes(stringWriter.GetStringBuilder().ToString()),
                             null));
                }
            }

            using (var connection = await this.connectionFactory())
            {
                await connection.AppendToStreamAsync(nameof(OutOfOffice), ExpectedVersion.Any, eventData).ConfigureAwait(false);
            }

            return outOfOffice;
        }

        public Task<OutOfOffice> GetAsync(int outOfOfficeId, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
