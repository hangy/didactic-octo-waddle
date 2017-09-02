namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using NodaTime;
    using System;

    public class UpdateOutOfOfficeEntryCommand : IRequest
    {
        public UpdateOutOfOfficeEntryCommand(int id, int userId, Interval interval, string reason)
        {
            this.Id = id;
            this.UserId = userId > 0 ? userId : throw new ArgumentOutOfRangeException(nameof(userId));
            this.Interval = interval != default ? interval : throw new ArgumentException($"interval invalid", nameof(interval));
            this.Reason = reason;
        }

        public int Id { get; }

        public int UserId { get; }

        public Interval Interval { get; }

        public string Reason { get; }
    }
}
