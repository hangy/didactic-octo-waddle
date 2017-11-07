namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using CalendarBackend.Domain.AggregatesModel.DutyAggregate;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddDutySubstitutionCommandHandler : ICancellableAsyncRequestHandler<AddDutySubstitutionCommand>
    {
        private readonly IDutyRepository dutyRepository;

        public AddDutySubstitutionCommandHandler(IDutyRepository dutyRepository)
        {
            this.dutyRepository = dutyRepository ?? throw new ArgumentNullException(nameof(dutyRepository));
        }

        public async Task Handle(AddDutySubstitutionCommand message, CancellationToken cancellationToken)
        {
            var duty = await this.dutyRepository.GetAsync(message.DutyId, cancellationToken).ConfigureAwait(false);
            if (duty == null)
            {
                throw new InvalidOperationException($"Duty entry with ID {message.DutyId} was not found");
            }

            duty.AddSubstitute(message.UserId, message.Interval);
            await this.dutyRepository.UpdateAsync(duty, cancellationToken).ConfigureAwait(false);
        }
    }
}
