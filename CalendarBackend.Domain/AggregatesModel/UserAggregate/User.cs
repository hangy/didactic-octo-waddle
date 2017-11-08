namespace CalendarBackend.Domain.AggregatesModel.UserAggregate
{
    using CalendarBackend.Domain.Events;
    using CalendarBackend.Domain.SeedWork;
    using System;
    using System.Collections.Generic;
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

            this.AddDomainEvent(new UserAddedEvent(this.Id, this.userName, this.displayName, this.mailAddress.Address, this.color));
        }

        /// <remark>
        /// Should only be used when build from the repository.
        /// Creating this object with the protected constructor and pppulating it through reflection (like Entity Framework)
        /// could fix this issue, but that's not a priority for me, right now.
        /// </remark>
        public User(Guid id, string userName, string displayName, MailAddress mailAddress, UserColor color, IEnumerable<IDomainEvent> domainEvents)
        {
            this.Id = id;
            this.userName = userName;
            this.displayName = displayName;
            this.mailAddress = mailAddress;
            this.color = color;

            foreach (var @event in domainEvents)
            {
                this.AddDomainEvent(@event);
            }
        }

        protected User()
        {
        }
    }
}
