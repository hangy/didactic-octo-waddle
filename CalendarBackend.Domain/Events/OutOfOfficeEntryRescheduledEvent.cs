namespace CalendarBackend.Domain.Events
{
    using NodaTime;
    using System;

    public class OutOfOfficeEntryRescheduledEvent : IDomainEvent
    {
        public OutOfOfficeEntryRescheduledEvent(Guid outOfOfficeId, Interval interval)
        {
            this.Id = Guid.NewGuid();
            this.OutOfOfficeId = outOfOfficeId;
            this.Interval = interval;
        }

        public Guid Id { get; }

        public Interval Interval { get; }

        public Guid OutOfOfficeId { get; }
    }
}
