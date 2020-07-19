namespace CalendarBackend.Domain.AggregatesModel.DutyAggregate
{
    using CalendarBackend.Domain.Events;
    using CalendarBackend.Domain.SeedWork;
    using NodaTime;
    using System;
    using System.Collections.Generic;

    public class Duty : Entity, IAggregateRoot
    {
        private readonly DateInterval interval;

        private readonly string name;

        public Duty(string name, DateInterval interval)
        {
            this.Id = Guid.NewGuid();
            this.name = name;
            this.interval = interval;

            this.AddDomainEvent(new DutyCreatedEvent(this.Id, this.name, this.interval));
        }

        /// <remark>
        /// Should only be used when build from the repository.
        /// Creating this object with the protected constructor and populating it through reflection (like Entity Framework)
        /// could fix this issue, but that's not a priority for me, right now.
        /// </remark>
        public Duty(Guid id, string name, DateInterval interval, IEnumerable<IDomainEvent> domainEvents)
        {
            this.Id = id;
            this.name = name;
            this.interval = interval;

            foreach (var @event in domainEvents)
            {
                this.AddDomainEvent(@event);
            }
        }

        protected Duty()
        {
        }

        public void AddSubstitute(string userId, DateInterval interval)
        {
            this.AddDomainEvent(new OnDutySubstitutedDefinedEvent(this.Id, userId, interval));
        }

        public void AddUser(string userId, LocalDate start)
        {
            this.AddDomainEvent(new UserAddedToDutyEvent(this.Id, userId, start));
        }

        public void RemoveUser(string userId, LocalDate end)
        {
            this.AddDomainEvent(new UserRemovedFromDutyEvent(this.Id, userId, end));
        }
    }
}