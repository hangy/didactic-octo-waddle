namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateOutOfOfficeEntryCommandHandler : ICancellableAsyncRequestHandler<UpdateOutOfOfficeEntryCommand>
    {
        public Task Handle(UpdateOutOfOfficeEntryCommand message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
