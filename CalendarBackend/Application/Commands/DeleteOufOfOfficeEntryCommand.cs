namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using System;

    public class DeleteOufOfOfficeEntryCommand : IRequest
    {
        public DeleteOufOfOfficeEntryCommand(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
