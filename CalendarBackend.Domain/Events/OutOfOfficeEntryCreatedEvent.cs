namespace CalendarBackend.Domain.Events
{
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using MediatR;
    using NodaTime;
    using System;

    public class OutOfOfficeEntryCreatedEvent : INotification
    {
        public OutOfOfficeEntryCreatedEvent(string userId, Interval interval, string reason)
        {
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.Interval = interval;
            this.Reason = reason;
        }

        public string UserId { get; }

        public Interval Interval { get; }

        public string Reason { get; }
    }
}
