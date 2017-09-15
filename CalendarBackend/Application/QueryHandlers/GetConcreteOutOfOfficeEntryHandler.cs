namespace CalendarBackend.Application.QueryHandlers
{
    using CalendarBackend.Application.Queries;
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetConcreteOutOfOfficeEntryHandler : ICancellableAsyncRequestHandler<GetConcreteOutOfOfficeEntry, OutOfOffice>
    {
        private readonly OutOfOfficeReadModel model;

        public GetConcreteOutOfOfficeEntryHandler(OutOfOfficeReadModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public async Task<OutOfOffice> Handle(GetConcreteOutOfOfficeEntry message, CancellationToken cancellationToken)
        {
            var entries = await this.model.GetEntriesAsync(cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            return entries.SingleOrDefault(e => e.Id == message.Id);
        }
    }
}
