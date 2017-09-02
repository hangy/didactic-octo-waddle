namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteOufOfOfficeEntryCommandHandler : ICancellableAsyncRequestHandler<DeleteOufOfOfficeEntryCommand>
    {
        public Task Handle(DeleteOufOfOfficeEntryCommand message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
