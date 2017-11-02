namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using NodaTime;
    using System;

    public class AddDutySubstitutionCommand : IRequest
    {
        public AddDutySubstitutionCommand(Guid dutyId, string userId, DateInterval interval)
        {
            this.DutyId = dutyId;
            this.UserId = userId;
            this.Interval = interval;
        }

        public Guid DutyId { get; }

        public DateInterval Interval { get; }

        public string UserId { get; }
    }
}
