namespace CalendarBackend.Domain.AggregatesModel.UserAggregate
{
    using CalendarBackend.Domain.Events;
    using CalendarBackend.Domain.SeedWork;
    using System;
    using System.Net.Mail;

    public class User : Entity, IAggregateRoot
    {
        private UserColor color;

        private string displayName;

        private MailAddress mailAddress;

        private string userName;

        public User(Guid id, string userName, string displayName, MailAddress mailAddress, UserColor color)
        {
            this.Id = id;
            this.userName = userName;
            this.displayName = displayName;
            this.mailAddress = mailAddress;
            this.color = color;

            this.AddDomainEvent(new UserAddedEvent(this.Id, this.userName, this.displayName, this.mailAddress, this.color));
        }

        protected User()
        {
        }
    }
}
