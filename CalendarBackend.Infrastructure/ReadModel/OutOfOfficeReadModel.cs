﻿namespace CalendarBackend.Infrastructure.ReadModel
{
    using CalendarBackend.Domain.Events;
    using CalendarBackend.Infrastructure.EventStore;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class OutOfOfficeReadModel : INotificationHandler<IDomainEvent>
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

        public async Task Handle(IDomainEvent notification, CancellationToken cancellationToken)
        {
            if (!this.initialized)
            {
                await this.InitializeAsync(cancellationToken).ConfigureAwait(false);
            }

            await this.HandleAsync(notification, cancellationToken).ConfigureAwait(false);
        }

        private void Handle(OutOfOfficeEntryCreatedEvent e)
        {
            this.entries.Add(new OutOfOffice { Id = e.OutOfOfficeId, UserId = e.UserId, Interval = e.Interval, Reason = e.Reason, DomainEvents = new List<IDomainEvent> { e } });
        }

        private void Handle(OutOfOfficeEntryCancelledEvent e)
        {
            var entry = this.entries.SingleOrDefault(ent => ent.Id == e.OutOfOfficeId);
            if (entry != null)
            {
                this.entries.Remove(entry);
            }
        }

        private void Handle(OutOfOfficeEntryRescheduledEvent e)
        {
            var entry = this.entries.SingleOrDefault(ent => ent.Id == e.OutOfOfficeId);
            if (entry != null)
            {
                entry.Interval = e.Interval;
            }
        }

        private void Handle(OutOfOfficeEntryRereasonedEvent e)
        {
            var entry = this.entries.SingleOrDefault(ent => ent.Id == e.OutOfOfficeId);
            if (entry != null)
            {
                entry.Reason = e.Reason;
            }
        }

        private Task HandleAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            switch (@event)
            {
                case OutOfOfficeEntryCreatedEvent e:
                    this.Handle(e);
                    break;
                case OutOfOfficeEntryCancelledEvent e:
                    this.Handle(e);
                    break;
                case OutOfOfficeEntryRescheduledEvent e:
                    this.Handle(e);
                    break;
                case OutOfOfficeEntryRereasonedEvent e:
                    this.Handle(e);
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
