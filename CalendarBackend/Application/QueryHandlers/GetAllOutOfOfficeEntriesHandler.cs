﻿namespace CalendarBackend.Application.QueryHandlers
{
    using CalendarBackend.Application.Queries;
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetAllOutOfOfficeEntriesHandler : IRequestHandler<GetAllOutOfOfficeEntries, IEnumerable<OutOfOffice>>
    {
        public GetAllOutOfOfficeEntriesHandler(OutOfOfficeReadModel model)
        {
            this.Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public OutOfOfficeReadModel Model { get; }

#pragma warning disable CC0061 // Asynchronous method can be terminated with the 'Async' keyword.
        public async Task<IEnumerable<OutOfOffice>> Handle(GetAllOutOfOfficeEntries message, CancellationToken cancellationToken = default)
#pragma warning restore CC0061 // Asynchronous method can be terminated with the 'Async' keyword.
        {
            return await Model.GetEntriesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
