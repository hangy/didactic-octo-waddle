namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using NodaTime;
    using System;

    public class AddOutOfOfficeEntryCommand : IRequest<string>
    {
        public AddOutOfOfficeEntryCommand(string userId, Interval interval, string reason)
        {
            this.UserId = !string.IsNullOrWhiteSpace(userId) ? userId : throw new ArgumentNullException(nameof(userId));
            this.Interval = interval != default ? interval : throw new ArgumentException($"interval invalid", nameof(interval));
            this.Reason = reason;
        }

        public string UserId { get; }
                
        public Interval Interval { get; }

        public string Reason { get; }
    }
}
