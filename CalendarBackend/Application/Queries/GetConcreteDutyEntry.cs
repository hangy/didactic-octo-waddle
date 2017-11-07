namespace CalendarBackend.Application.Queries
{
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using System;

    public class GetConcreteDutyEntry : IRequest<Duty>
    {
        public GetConcreteDutyEntry(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
