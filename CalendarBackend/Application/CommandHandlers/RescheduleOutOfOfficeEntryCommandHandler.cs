namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RescheduleOutOfOfficeEntryCommandHandler : ICancellableAsyncRequestHandler<RescheduleOutOfOfficeEntryCommand>
    {
        private readonly IOutOfOfficeRepository outOfOfficeRepository;

        public RescheduleOutOfOfficeEntryCommandHandler(IOutOfOfficeRepository outOfOfficeRepository)
        {
            this.outOfOfficeRepository = outOfOfficeRepository ?? throw new ArgumentNullException(nameof(outOfOfficeRepository));
        }

        public async Task Handle(RescheduleOutOfOfficeEntryCommand message, CancellationToken cancellationToken)
        {
            var outOfOffice = await this.outOfOfficeRepository.GetAsync(message.Id, cancellationToken).ConfigureAwait(false);
            if (outOfOffice == null)
            {
                throw new InvalidOperationException($"OutOfOffice entry with ID {message.Id} was not found");
            }

            outOfOffice.RescheduleTo(message.Interval);
            await this.outOfOfficeRepository.UpdateAsync(outOfOffice, cancellationToken).ConfigureAwait(false);
        }
    }
}
