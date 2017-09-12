namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using CalendarBackend.Infrastructure.EventStore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class OutOfOfficeRepository : IOutOfOfficeRepository
    {
        private readonly IEventStream eventStream;

        public OutOfOfficeRepository(IEventStream eventStream)
        {
            this.eventStream = eventStream ?? throw new ArgumentNullException(nameof(eventStream));
        }

        public async Task<OutOfOffice> AddAsync(OutOfOffice outOfOffice, CancellationToken cancellationToken = default)
        {
            if (outOfOffice == null)
            {
                throw new ArgumentNullException(nameof(outOfOffice));
            }

            await this.eventStream.WriteEventsAsync(outOfOffice.DomainEvents, cancellationToken).ConfigureAwait(false);
            return outOfOffice;
        }

        public Task<OutOfOffice> GetAsync(int outOfOfficeId, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
