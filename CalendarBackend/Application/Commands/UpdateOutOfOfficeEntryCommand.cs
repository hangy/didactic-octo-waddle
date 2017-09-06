namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using NodaTime;
    using System;

    public class UpdateOutOfOfficeEntryCommand : IRequest
    {
        public UpdateOutOfOfficeEntryCommand(string id, string userId, Interval interval, string reason)
        {
            this.Id = id;
            this.UserId = !string.IsNullOrWhiteSpace(userId) ? userId : throw new ArgumentNullException(nameof(userId));
            this.Interval = interval != default ? interval : throw new ArgumentException($"interval invalid", nameof(interval));
            this.Reason = reason;
        }

        public string Id { get; }

        public Interval Interval { get; }

        public string Reason { get; }

        public string UserId { get; }
    }
}
