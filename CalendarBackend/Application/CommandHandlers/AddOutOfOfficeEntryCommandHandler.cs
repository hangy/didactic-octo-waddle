namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddOutOfOfficeEntryCommandHandler : ICancellableAsyncRequestHandler<AddOutOfOfficeEntryCommand, string>
    {
        private readonly IOutOfOfficeRepository outOfOfficeRepository;

        public AddOutOfOfficeEntryCommandHandler(IOutOfOfficeRepository outOfOfficeRepository)
        {
            this.outOfOfficeRepository = outOfOfficeRepository ?? throw new ArgumentNullException(nameof(outOfOfficeRepository));
        }

        public async Task<string> Handle(AddOutOfOfficeEntryCommand message, CancellationToken cancellationToken)
        {
            var aggregate = await this.outOfOfficeRepository.AddAsync(new OutOfOffice(null, message.UserId, message.Interval, message.Reason), cancellationToken).ConfigureAwait(false);
            await this.outOfOfficeRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return aggregate.Id;
        }
    }
}
