namespace CalendarBackend.Application.QueryHandlers
{
    using CalendarBackend.Application.Models;
    using CalendarBackend.Application.Queries;
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetConcreteOutOfOfficeEntryHandler : ICancellableAsyncRequestHandler<GetConcreteOutOfOfficeEntry, Models.OutOfOffice>
    {
        private readonly IOutOfOfficeRepository outOfOfficeRepository;

        public GetConcreteOutOfOfficeEntryHandler(IOutOfOfficeRepository outOfOfficeRepository)
        {
            this.outOfOfficeRepository = outOfOfficeRepository ?? throw new ArgumentNullException(nameof(outOfOfficeRepository));
        }

        public async Task<Models.OutOfOffice> Handle(GetConcreteOutOfOfficeEntry message, CancellationToken cancellationToken)
        {
            var entity = await this.outOfOfficeRepository.GetAsync(message.Id).ConfigureAwait(false);
            if (entity == null)
            {
                throw new KeyNotFoundException();
            }

            return new Models.OutOfOffice
            {
                Id = entity.Id
            };
        }
    }
}
