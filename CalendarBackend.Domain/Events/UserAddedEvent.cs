namespace CalendarBackend.Domain.Events
{
    using CalendarBackend.Domain.AggregatesModel.UserAggregate;
    using System;
    using System.Net.Mail;

    public class UserAddedEvent : IDomainEvent
    {
        public UserAddedEvent(Guid id, string userName, string displayName, MailAddress mailAddress, UserColor color)
        {
            this.Id = id;
            this.UserName = userName;
            this.DisplayName = displayName;
            this.MailAddress = mailAddress;
            this.Color = color;
        }

        public UserColor Color { get; }

        public string DisplayName { get; }

        public Guid Id { get; }

        public MailAddress MailAddress { get; }

        public string UserName { get; }
    }
}
