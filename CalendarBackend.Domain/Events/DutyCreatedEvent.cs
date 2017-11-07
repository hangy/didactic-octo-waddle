namespace CalendarBackend.Domain.Events
{
    using NodaTime;
    using System;

    public class DutyCreatedEvent : IDomainEvent
    {
        public DutyCreatedEvent(Guid dutyId, string name, DateInterval interval)
        {
            this.Id = Guid.NewGuid();
            this.DutyId = dutyId;
            this.Name = name;
            this.Interval = interval;
        }

        public Guid DutyId { get; }

        public Guid Id { get; }

        public DateInterval Interval { get; }

        public string Name { get; }
    }
}
