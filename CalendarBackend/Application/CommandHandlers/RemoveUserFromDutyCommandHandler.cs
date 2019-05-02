namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using CalendarBackend.Domain.AggregatesModel.DutyAggregate;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemoveUserFromDutyCommandHandler : IRequestHandler<RemoveUserFromDutyCommand>
    {
        private readonly IDutyRepository dutyRepository;

        public RemoveUserFromDutyCommandHandler(IDutyRepository dutyRepository)
        {
            this.dutyRepository = dutyRepository ?? throw new ArgumentNullException(nameof(dutyRepository));
        }

        public async Task<Unit> Handle(RemoveUserFromDutyCommand message, CancellationToken cancellationToken)
        {
            var duty = await this.dutyRepository.GetAsync(message.DutyId, cancellationToken).ConfigureAwait(false);
            if (duty == null)
            {
                throw new InvalidOperationException($"Duty entry with ID {message.DutyId} was not found");
            }

            duty.RemoveUser(message.UserId, message.End);
            _ = await this.dutyRepository.UpdateAsync(duty, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
