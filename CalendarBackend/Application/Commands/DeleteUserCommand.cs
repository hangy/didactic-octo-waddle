namespace CalendarBackend.Application.Commands
{
    using MediatR;

    public class DeleteUserCommand : IRequest
    {
        public DeleteUserCommand(int id)
        {
            this.Id = id;
        }

        public int Id { get; }
    }
}
