namespace CalendarBackend.Infrastructure.ReadModel
{
    using CalendarBackend.Domain.Events;
    using CalendarBackend.Infrastructure.EventStore;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class UserReadModel : IAsyncNotificationHandler<IDomainEvent>
    {
        private readonly IList<User> entries = new List<User>();

        private readonly IEventStream eventStream;

        private bool initialized;

        public UserReadModel(IEventStream eventStream)
        {
            this.eventStream = eventStream ?? throw new ArgumentNullException(nameof(eventStream));
        }

        public async Task<IEnumerable<User>> GetEntriesAsync(CancellationToken cancellationToken = default)
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
                case UserAddedEvent e:
                    this.entries.Add(new User { Id = e.Id, UserName = e.UserName, DisplayName = e.DisplayName, MailAddress = e.MailAddress?.Address, Color = e.Color?.Color });
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
