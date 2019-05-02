namespace CalendarBackend.Infrastructure.ReadModel
{
    using CalendarBackend.Domain.Events;
    using CalendarBackend.Infrastructure.EventStore;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class UserReadModel : INotificationHandler<IDomainEvent>
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

        public async Task Handle(IDomainEvent notification, CancellationToken cancellationToken)
        {
            if (!this.initialized)
            {
                await this.InitializeAsync(cancellationToken).ConfigureAwait(false);
            }

            await this.HandleAsync(notification, cancellationToken).ConfigureAwait(false);
        }

        private Task HandleAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            switch (@event)
            {
                case UserAddedEvent e:
                    this.entries.Add(new User { Id = e.Id, UserName = e.UserName, DisplayName = e.DisplayName, MailAddress = e.MailAddress, Color = e.Color?.Color });
                    break;
            }

            return Task.CompletedTask;
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
