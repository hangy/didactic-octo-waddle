namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using NodaTime;
    using System;

    public class AddOutOfOfficeEntryCommand : IRequest<int>
    {
        public AddOutOfOfficeEntryCommand(int userId, Interval interval, string reason)
        {
            this.UserId = userId > 0 ? userId : throw new ArgumentOutOfRangeException(nameof(userId));
            this.Interval = interval != default ? interval : throw new ArgumentException($"interval invalid", nameof(interval));
            this.Reason = reason;
        }

        public int UserId { get; }

        
        public Interval Interval { get; }

        public string Reason { get; }
    }
}
