namespace CalendarBackend.Application.Commands
{
    using MediatR;

    public class AddUserCommand : IRequest<int>
    {
        public AddUserCommand(string userName, string displayName, string color, string mailAddress)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Color = color;
            this.MailAddress = mailAddress;
        }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Color { get; set; }

        public string MailAddress { get; set; }
    }
}
