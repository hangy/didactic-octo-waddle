namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using NodaTime;
    using System;

    public class RemoveUserFromDutyCommand : IRequest
    {
        public RemoveUserFromDutyCommand(Guid dutyId, string userId, LocalDate end)
        {
            this.DutyId = dutyId;
            this.UserId = userId;
            this.End = end;
        }

        public Guid DutyId { get; }

        public LocalDate End { get; }

        public string UserId { get; }
    }
}
