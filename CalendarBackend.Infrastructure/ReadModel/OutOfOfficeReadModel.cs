namespace CalendarBackend.Infrastructure.ReadModel
{
    using CalendarBackend.Domain.Events;
    using CalendarBackend.Infrastructure.EventStore;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class OutOfOfficeReadModel : IAsyncNotificationHandler<IDomainEvent>
    {
        private readonly IList<OutOfOffice> entries = new List<OutOfOffice>();

        private readonly IEventStream eventStream;

        private bool initialized;

        public OutOfOfficeReadModel(IEventStream eventStream)
        {
            this.eventStream = eventStream ?? throw new ArgumentNullException(nameof(eventStream));
        }

        public async Task<IEnumerable<OutOfOffice>> GetEntriesAsync(CancellationToken cancellationToken = default)
        {
            if (!this.initialized)
            {
                await this.InitializeAsync(cancellationToken).ConfigureAwait(false);
            }

            return this.entries;
        }

        public async Task Handle(IDomainEvent notification)
        {
            if (!this.initialized)
            {
                await this.InitializeAsync().ConfigureAwait(false);
            }

            await this.HandleAsync(notification);
        }

        private async Task HandleAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
        {
            switch (@event)
            {
                case OutOfOfficeEntryCreatedEvent e:
                    this.entries.Add(new OutOfOffice { Id = e.OutOfOfficeId, UserId = e.UserId, Interval = e.Interval, Reason = e.Reason });
                    break;
            }
        }

        private async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var events = await this.eventStream.ReadAllEventsAsync(cancellationToken).ConfigureAwait(false);
            foreach (var @event in events)
            {
                await this.HandleAsync(@event, cancellationToken).ConfigureAwait(false);
            }

            this.initialized = true;
        }
    }
}
