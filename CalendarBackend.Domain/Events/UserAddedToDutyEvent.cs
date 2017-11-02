namespace CalendarBackend.Domain.Events
{
    using NodaTime;
    using System;

    public class UserAddedToDutyEvent : IDomainEvent
    {
        public UserAddedToDutyEvent(Guid dutyId, string userId, LocalDate start)
        {
            this.Id = Guid.NewGuid();
            this.DutyId = dutyId;
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.Start = start;
        }

        public Guid DutyId { get; }

        public Guid Id { get; }

        public LocalDate Start { get; }

        public string UserId { get; }
    }
}
