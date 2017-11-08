namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using CalendarBackend.Domain.AggregatesModel.DutyAggregate;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddDutyEntryCommandHandler : ICancellableAsyncRequestHandler<AddDutyEntryCommand, Guid>
    {
        private readonly IDutyRepository dutyRepository;

        public AddDutyEntryCommandHandler(IDutyRepository dutyRepository)
        {
            this.dutyRepository = dutyRepository ?? throw new ArgumentNullException(nameof(dutyRepository));
        }

        public async Task<Guid> Handle(AddDutyEntryCommand message, CancellationToken cancellationToken)
        {
            var aggregate = await this.dutyRepository.AddAsync(new Duty(message.Name, message.Interval), cancellationToken).ConfigureAwait(false);

            return aggregate.Id;
        }
    }
}
