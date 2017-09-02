namespace CalendarBackend.Application.Commands
{
    using MediatR;

    public class DeleteOufOfOfficeEntryCommand : IRequest
    {
        public DeleteOufOfOfficeEntryCommand(int id)
        {
            this.Id = id;
        }

        public int Id { get; }
    }
}
