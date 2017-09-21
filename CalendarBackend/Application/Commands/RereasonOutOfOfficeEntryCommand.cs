namespace CalendarBackend.Application.Commands
{
    using MediatR;
    using System;

    public class RereasonOutOfOfficeEntryCommand : IRequest
    {
        public RereasonOutOfOfficeEntryCommand(Guid id, string reason)
        {
            this.Id = id;
            this.Reason = reason;
        }

        public Guid Id { get; }

        public string Reason { get; }
    }
}
