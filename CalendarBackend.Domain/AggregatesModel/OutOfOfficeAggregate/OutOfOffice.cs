namespace CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate
{
    using CalendarBackend.Domain.Events;
    using CalendarBackend.Domain.SeedWork;
    using NodaTime;

    public class OutOfOffice : Entity, IAggregateRoot
    {
        private string userId;

        private Interval interval;

        private string reason;

        public OutOfOffice(string id, string userId, Interval interval, string reason)
        {
            this.Id = id;
            this.userId = userId;
            this.interval = interval;
            this.reason = reason;

            this.AddDomainEvent(new OutOfOfficeEntryCreatedEvent(this.userId, this.interval, this.reason));
        }
    }
}