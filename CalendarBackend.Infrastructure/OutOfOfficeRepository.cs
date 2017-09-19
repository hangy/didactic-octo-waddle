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
            return entry == null ? null : new OutOfOffice(entry.Id, entry.UserId, entry.Interval, entry.Reason, entry.DomainEvents);
        }

        public async Task<int> UpdateAsync(OutOfOffice outOfOffice, CancellationToken cancellationToken = default)
        {
            if (outOfOffice == null)
            {
                throw new ArgumentNullException(nameof(outOfOffice));
            }

            // We currently don't have a real change tracker, so just get the current entity and add new events.
            var current = await this.GetAsync(outOfOffice.Id, cancellationToken).ConfigureAwait(false);
            if (current == null)
            {
                await this.AddAsync(outOfOffice, cancellationToken).ConfigureAwait(false);
                return 1;
            }

            var newEvents = outOfOffice.DomainEvents.Except(current.DomainEvents, new DomainEventComparer());
            return await this.eventStream.WriteEventsAsync(newEvents, cancellationToken).ConfigureAwait(false);
        }
    }
}