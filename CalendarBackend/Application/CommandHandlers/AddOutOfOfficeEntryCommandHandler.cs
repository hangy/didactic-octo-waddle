﻿namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddOutOfOfficeEntryCommandHandler : IRequestHandler<AddOutOfOfficeEntryCommand, Guid>
    {
        private readonly IOutOfOfficeRepository outOfOfficeRepository;

        public AddOutOfOfficeEntryCommandHandler(IOutOfOfficeRepository outOfOfficeRepository)
        {
            this.outOfOfficeRepository = outOfOfficeRepository ?? throw new ArgumentNullException(nameof(outOfOfficeRepository));
        }

        public async Task<Guid> Handle(AddOutOfOfficeEntryCommand message, CancellationToken cancellationToken)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var aggregate = await this.outOfOfficeRepository.AddAsync(new OutOfOffice(message.UserId, message.Interval, message.Reason), cancellationToken).ConfigureAwait(false);
            
            return aggregate.Id;
        }
    }
}
