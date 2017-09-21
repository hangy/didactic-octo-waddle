namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using NodaTime;
    using System;

    public class RescheduleOutOfOfficeEntryCommand : IRequest
    {
        public RescheduleOutOfOfficeEntryCommand(Guid id, Interval interval)
        {
            this.Id = id;
            this.Interval = interval != default ? interval : throw new ArgumentException($"interval invalid", nameof(interval));
        }

        public Guid Id { get; }

        public Interval Interval { get; }
    }
}
