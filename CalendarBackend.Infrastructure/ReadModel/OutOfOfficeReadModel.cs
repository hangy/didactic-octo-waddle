namespace CalendarBackend.Infrastructure.ReadModel
{
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using CalendarBackend.Domain.Events;
    using CalendarBackend.Infrastructure.EventStore;
    using MediatR;
    using System;
    using System.Collections.Generic;

    public class OutOfOfficeReadModel : INotificationHandler<IDomainEvent>
    {
        private readonly IEventStream eventStream;

        public OutOfOfficeReadModel(IEventStream eventStream)
        {
            this.eventStream = eventStream ?? throw new ArgumentNullException(nameof(eventStream));
        }

        public IReadOnlyList<OutOfOffice> Entries => new List<OutOfOffice>();

        public void Handle(IDomainEvent notification)
        {
            throw new NotImplementedException();
        }
    }
}
