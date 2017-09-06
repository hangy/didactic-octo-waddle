namespace CalendarBackend.Application.Commands
{
    using MediatR;

    public class UpdateUserCommand : IRequest
    {
        public UpdateUserCommand(string id, string userName, string displayName, string color, string mailAddress)
        {
            this.Id = id;
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Color = color;
            this.MailAddress = mailAddress;
        }

        public string Color { get; set; }

        public string DisplayName { get; set; }

        public string Id { get; set; }

        public string MailAddress { get; set; }

        public string UserName { get; set; }
    }
}
