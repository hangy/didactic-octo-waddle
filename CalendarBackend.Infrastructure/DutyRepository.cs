namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.AggregatesModel.DutyAggregate;
    using CalendarBackend.Infrastructure.EventStore;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class DutyRepository : IDutyRepository
    {
        private readonly IEventStream eventStream;

        private readonly ReadModel.DutyReadModel readModel;

        public DutyRepository(IEventStream eventStream, ReadModel.DutyReadModel readModel)
        {
            this.eventStream = eventStream ?? throw new ArgumentNullException(nameof(eventStream));
            this.readModel = readModel ?? throw new ArgumentNullException(nameof(readModel));
        }

        public async Task<Duty> AddAsync(Duty duty, CancellationToken cancellationToken = default)
        {
            await this.eventStream.WriteEventsAsync(duty.DomainEvents, cancellationToken).ConfigureAwait(false);
            return duty;
        }

        public async Task<Duty?> GetAsync(Guid dutyId, CancellationToken cancellationToken = default)
        {
            var entries = await this.readModel.GetEntriesAsync(cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            var entry = entries.SingleOrDefault(e => e.Id == dutyId);
            return entry == null ? null : new Duty(entry.Id, entry.Name, entry.Interval, entry.DomainEvents);
        }

        public async Task<int> UpdateAsync(Duty duty, CancellationToken cancellationToken = default)
        {
            // We currently don't have a real change tracker, so just get the current entity and add new events.
            var current = await this.GetAsync(duty.Id, cancellationToken).ConfigureAwait(false);
            if (current == null)
            {
                await this.AddAsync(duty, cancellationToken).ConfigureAwait(false);
                return 1;
            }

            var newEvents = duty.DomainEvents.Except(current.DomainEvents, new DomainEventComparer());
            return await this.eventStream.WriteEventsAsync(newEvents, cancellationToken).ConfigureAwait(false);
        }
    }
}