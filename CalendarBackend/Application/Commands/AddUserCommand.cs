namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using System;

    public class AddUserCommand : IRequest<Guid>
    {
        public AddUserCommand(string userName, string displayName, string color, string mailAddress)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Color = color;
            this.MailAddress = mailAddress;
        }

        public string Color { get; set; }

        public string DisplayName { get; set; }

        public string MailAddress { get; set; }

        public string UserName { get; set; }
    }
}
