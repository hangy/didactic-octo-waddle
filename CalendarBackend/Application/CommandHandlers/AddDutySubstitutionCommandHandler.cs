﻿namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using CalendarBackend.Domain.AggregatesModel.DutyAggregate;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddDutySubstitutionCommandHandler : IRequestHandler<AddDutySubstitutionCommand>
    {
        private readonly IDutyRepository dutyRepository;

        public AddDutySubstitutionCommandHandler(IDutyRepository dutyRepository)
        {
            this.dutyRepository = dutyRepository ?? throw new ArgumentNullException(nameof(dutyRepository));
        }

        public async Task<Unit> Handle(AddDutySubstitutionCommand message, CancellationToken cancellationToken)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var duty = await this.dutyRepository.GetAsync(message.DutyId, cancellationToken).ConfigureAwait(false);
            if (duty == null)
            {
                throw new InvalidOperationException($"Duty entry with ID {message.DutyId} was not found");
            }

            duty.AddSubstitute(message.UserId, message.Interval);
            _ = await this.dutyRepository.UpdateAsync(duty, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
