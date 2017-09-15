namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using CalendarBackend.Infrastructure.EventStore;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class OutOfOfficeRepository : IOutOfOfficeRepository
    {
        private readonly IEventStream eventStream;

        private readonly ReadModel.OutOfOfficeReadModel readModel;

        public OutOfOfficeRepository(IEventStream eventStream, ReadModel.OutOfOfficeReadModel readModel)
        {
            this.eventStream = eventStream ?? throw new ArgumentNullException(nameof(eventStream));
            this.readModel = readModel ?? throw new ArgumentNullException(nameof(readModel));
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

        public async Task<OutOfOffice> GetAsync(Guid outOfOfficeId, CancellationToken cancellationToken = default)
        {
            var entries = await this.readModel.GetEntriesAsync(cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            var entry = entries.SingleOrDefault(e => e.Id == outOfOfficeId);
            return entry == null ? null : new OutOfOffice(entry.Id, entry.UserId, entry.Interval, entry.Reason);
        }
    }
}
