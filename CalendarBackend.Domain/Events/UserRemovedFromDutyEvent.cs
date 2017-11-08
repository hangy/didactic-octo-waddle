namespace CalendarBackend.Domain.Events
{
    using NodaTime;
    using System;

    public class UserRemovedFromDutyEvent : IDomainEvent
    {
        public UserRemovedFromDutyEvent(Guid dutyId, string userId, LocalDate end)
        {
            this.Id = Guid.NewGuid();
            this.DutyId = dutyId;
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.End = end;
        }

        public Guid DutyId { get; }

        public LocalDate End { get; }

        public Guid Id { get; }

        public string UserId { get; }
    }
}
