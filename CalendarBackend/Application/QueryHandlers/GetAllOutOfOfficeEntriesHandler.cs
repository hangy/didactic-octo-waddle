namespace CalendarBackend.Application.QueryHandlers
{
    using CalendarBackend.Application.Models;
    using CalendarBackend.Application.Queries;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetAllOutOfOfficeEntriesHandler : ICancellableAsyncRequestHandler<GetAllOutOfOfficeEntries, IEnumerable<OutOfOffice>>
    {
        public GetAllOutOfOfficeEntriesHandler(IConfiguration configuration)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfiguration Configuration { get; }

        public Task<IEnumerable<OutOfOffice>> Handle(GetAllOutOfOfficeEntries message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
