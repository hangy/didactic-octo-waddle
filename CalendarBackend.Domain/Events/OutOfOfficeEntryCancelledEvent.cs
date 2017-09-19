namespace CalendarBackend.Domain.Events
{
    using System;

    public class OutOfOfficeEntryCancelledEvent : IDomainEvent
    {
        public OutOfOfficeEntryCancelledEvent(Guid outOfOfficeId)
        {
            this.Id = Guid.NewGuid();
            this.OutOfOfficeId = outOfOfficeId;
        }

        public Guid Id { get; }

        public Guid OutOfOfficeId { get; }
    }
}
