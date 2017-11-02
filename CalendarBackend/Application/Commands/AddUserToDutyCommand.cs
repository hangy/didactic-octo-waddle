namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using NodaTime;
    using System;

    public class AddUserToDutyCommand : IRequest
    {
        public AddUserToDutyCommand(Guid dutyId, string userId, LocalDate start)
        {
            this.DutyId = dutyId;
            this.UserId = userId;
            this.Start = start;
        }

        public Guid DutyId { get; }

        public LocalDate Start { get; }

        public string UserId { get; }
    }
}
