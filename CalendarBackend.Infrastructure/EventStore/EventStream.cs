namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class EventStream : IEventStream
    {
        private readonly IEventStore eventStore;

        private readonly IMediator mediator;

        public EventStream(IEventStore eventStore, IMediator mediator)
        {
            this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IReadOnlyList<IDomainEvent>> ReadAllEventsAsync(CancellationToken cancellationToken = default)
        {
            using (var eventReader = await this.eventStore.GetReaderAsync(cancellationToken).ConfigureAwait(false))
            {
                return await eventReader.ReadAllAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<int> WriteEventsAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
        {
            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            IReadOnlyList<IDomainEvent> writtenEvents;
            using (var eventWriter = await this.eventStore.GetWriterAsync(cancellationToken).ConfigureAwait(false))
            {
                writtenEvents = await eventWriter.AppendAllAsync(events, cancellationToken).ConfigureAwait(false);
            }

            await this.PublishEventsAsync(writtenEvents, cancellationToken).ConfigureAwait(false);
            return writtenEvents.Count;
        }

        private async Task PublishEventsAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken)
        {
            foreach (var @event in events)
            {
                await this.mediator.Publish(@event, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
