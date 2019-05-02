namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RereasonOutOfOfficeEntryCommandHandler : IRequestHandler<RereasonOutOfOfficeEntryCommand>
    {
        private readonly IOutOfOfficeRepository outOfOfficeRepository;

        public RereasonOutOfOfficeEntryCommandHandler(IOutOfOfficeRepository outOfOfficeRepository)
        {
            this.outOfOfficeRepository = outOfOfficeRepository ?? throw new ArgumentNullException(nameof(outOfOfficeRepository));
        }

        public async Task<Unit> Handle(RereasonOutOfOfficeEntryCommand message, CancellationToken cancellationToken)
        {
            var outOfOffice = await this.outOfOfficeRepository.GetAsync(message.Id, cancellationToken).ConfigureAwait(false);
            if (outOfOffice == null)
            {
                throw new InvalidOperationException($"OutOfOffice entry with ID {message.Id} was not found");
            }

            outOfOffice.ChangeReason(message.Reason);
            await this.outOfOfficeRepository.UpdateAsync(outOfOffice, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
