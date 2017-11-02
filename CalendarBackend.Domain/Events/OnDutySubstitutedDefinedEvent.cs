namespace CalendarBackend.Domain.Events
{
    using NodaTime;
    using System;

    public class OnDutySubstitutedDefinedEvent : IDomainEvent
    {
        public OnDutySubstitutedDefinedEvent(Guid dutyId, string userId, DateInterval interval)
        {
            this.Id = Guid.NewGuid();
            this.DutyId = dutyId;
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.Interval = interval;
        }

        public Guid DutyId { get; }

        public Guid Id { get; }

        public DateInterval Interval { get; }

        public string UserId { get; }
    }
}
