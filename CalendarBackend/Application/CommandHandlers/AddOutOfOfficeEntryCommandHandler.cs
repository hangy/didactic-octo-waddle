namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddOutOfOfficeEntryCommandHandler : ICancellableAsyncRequestHandler<AddOutOfOfficeEntryCommand, int>
    {
        public Task<int> Handle(AddOutOfOfficeEntryCommand message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
