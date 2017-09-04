namespace CalendarBackend.Application.Commands
{
    using MediatR;

    public class DeleteUserCommand : IRequest
    {
        public DeleteUserCommand(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
    }
}
