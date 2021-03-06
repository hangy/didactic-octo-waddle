﻿namespace CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate
{
    using CalendarBackend.Domain.Events;
    using CalendarBackend.Domain.SeedWork;
    using NodaTime;
    using System;
    using System.Collections.Generic;

    public class OutOfOffice : Entity, IAggregateRoot
    {
        private Interval interval;

        private string reason;

        private string userId;

        public OutOfOffice(string userId, Interval interval, string reason)
        {
            this.Id = Guid.NewGuid();
            this.userId = userId;
            this.interval = interval;
            this.reason = reason;

            this.AddDomainEvent(new OutOfOfficeEntryCreatedEvent(this.Id, this.userId, this.interval, this.reason));
        }

        /// <remark>
        /// Should only be used when build from the repository.
        /// Creating this object with the protected constructor and pppulating it through reflection (like Entity Framework)
        /// could fix this issue, but that's not a priority for me, right now.
        /// </remark>
        public OutOfOffice(Guid id, string userId, Interval interval, string reason, IEnumerable<IDomainEvent> domainEvents)
        {
            this.Id = id;
            this.userId = userId;
            this.interval = interval;
            this.reason = reason;

            if (domainEvents == null)
            {
                return;
            }

            foreach (var @event in domainEvents)
            {
                this.AddDomainEvent(@event);
            }
        }

        public void ChangeReason(string reason)
        {
            this.reason = reason;
            this.AddDomainEvent(new OutOfOfficeEntryRereasonedEvent(this.Id, reason));
        }

        public void RescheduleTo(Interval interval)
        {
            this.interval = interval;
            this.AddDomainEvent(new OutOfOfficeEntryRescheduledEvent(this.Id, interval));
        }

        protected OutOfOffice()
        {
        }

        public void SetCancelledStatus()
        {
            this.AddDomainEvent(new OutOfOfficeEntryCancelledEvent(this.Id));
        }
    }
}