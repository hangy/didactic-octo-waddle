namespace CalendarBackend.Application.QueryHandlers
{
    using CalendarBackend.Application.Queries;
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using NodaTime;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetConcreteDutyEntryHandler : IRequestHandler<GetConcreteDutyEntry, Duty>
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
            return entries
                .Select(e => RemoveOutOfRangeAssignments(e, message.StartAppointmentRange, message.EndAppointmentRange))
                .SingleOrDefault(e => e.Id == message.Id);
        }

        private static Duty RemoveOutOfRangeAssignments(Duty duty, LocalDate? startAppointmentRange, LocalDate? endAppointmentRange)
        {
            if (startAppointmentRange.HasValue)
            {
                duty.Assignments.RemoveAll(a => a.Interval.Start < startAppointmentRange.Value);
            }

            if (endAppointmentRange.HasValue)
            {
                duty.Assignments.RemoveAll(a => a.Interval.End > endAppointmentRange.Value);
            }

            return duty;
        }
    }
}
