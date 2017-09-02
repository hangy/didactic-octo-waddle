namespace CalendarBackend.Application.QueryHandlers
{
    using CalendarBackend.Application.Models;
    using CalendarBackend.Application.Queries;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetConcreteOutOfOfficeEntryHandler : ICancellableAsyncRequestHandler<GetConcreteOutOfOfficeEntry, OutOfOffice>
    {
        public Task<OutOfOffice> Handle(GetConcreteOutOfOfficeEntry message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
