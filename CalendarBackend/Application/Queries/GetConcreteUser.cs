namespace CalendarBackend.Application.Queries
{
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using System;

    public record GetConcreteUser : IRequest<User?>
    {
        public GetConcreteUser(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
