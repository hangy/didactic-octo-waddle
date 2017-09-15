namespace CalendarBackend.Application.Queries
{
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using System;

    public class GetConcreteOutOfOfficeEntry : IRequest<OutOfOffice>
    {
        public GetConcreteOutOfOfficeEntry(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
