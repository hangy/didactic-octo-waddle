namespace CalendarBackend.Application.Commands
{
    using MediatR;

    public class DeleteOufOfOfficeEntryCommand : IRequest
    {
        public DeleteOufOfOfficeEntryCommand(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
    }
}
