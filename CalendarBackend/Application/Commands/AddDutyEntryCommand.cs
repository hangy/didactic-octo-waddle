namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using NodaTime;
    using System;

    public class AddDutyEntryCommand : IRequest<Guid>
    {
        public AddDutyEntryCommand(string name, DateInterval interval)
        {
            this.Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
            this.Interval = interval != default ? interval : throw new ArgumentException($"interval invalid", nameof(interval));
        }

        public DateInterval Interval { get; }

        public string Name { get; }
    }
}
