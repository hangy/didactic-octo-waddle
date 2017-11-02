namespace CalendarBackend.Application.QueryHandlers
{
    using CalendarBackend.Application.Queries;
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetConcreteDutyEntryHandler : ICancellableAsyncRequestHandler<GetConcreteDutyEntry, Duty>
    {
        private readonly DutyReadModel model;

        public GetConcreteDutyEntryHandler(DutyReadModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public async Task<Duty> Handle(GetConcreteDutyEntry message, CancellationToken cancellationToken)
        {
            var entries = await this.model.GetEntriesAsync(cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            return entries.SingleOrDefault(e => e.Id == message.Id);
        }
    }
}
