namespace CalendarBackend.Domain.Events
{
    using NodaTime;
    using System;

    public class OutOfOfficeEntryRereasonedEvent : IDomainEvent
    {
        public OutOfOfficeEntryRereasonedEvent(Guid outOfOfficeId, string reason)
        {
            this.Id = Guid.NewGuid();
            this.OutOfOfficeId = outOfOfficeId;
            this.Reason = reason;
        }

        public Guid Id { get; }

        public Guid OutOfOfficeId { get; }

        public string Reason { get; }
    }
}
