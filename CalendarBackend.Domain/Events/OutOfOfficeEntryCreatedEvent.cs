namespace CalendarBackend.Domain.Events
{
    using NodaTime;
    using System;

    public class OutOfOfficeEntryCreatedEvent : IDomainEvent
    {
        public OutOfOfficeEntryCreatedEvent(Guid outOfOfficeId, string userId, Interval interval, string reason)
        {
            this.Id = Guid.NewGuid();
            this.OutOfOfficeId = outOfOfficeId;
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.Interval = interval;
            this.Reason = reason;
        }

        public Guid Id { get; }

        public Interval Interval { get; }

        public Guid OutOfOfficeId { get; }

        public string Reason { get; }

        public string UserId { get; }
    }
}
