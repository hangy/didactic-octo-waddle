namespace CalendarBackend.Application.QueryHandlers
{
    using CalendarBackend.Application.Queries;
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetConcreteOutOfOfficeEntryHandler : ICancellableAsyncRequestHandler<GetConcreteOutOfOfficeEntry, Models.OutOfOffice>
    {
        private readonly OutOfOfficeReadModel model;

        public GetConcreteOutOfOfficeEntryHandler(OutOfOfficeReadModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public async Task<Models.OutOfOffice> Handle(GetConcreteOutOfOfficeEntry message, CancellationToken cancellationToken)
        {
            return this.model.Entries.OfType<Models.OutOfOffice>().FirstOrDefault();
        }
    }
}
