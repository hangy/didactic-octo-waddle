namespace CalendarBackend.Application.Queries
{
    using CalendarBackend.Application.Models;
    using MediatR;

    public class GetConcreteUser : IRequest<User>
    {
        public GetConcreteUser(int id)
        {
            this.Id = id;
        }

        public int Id { get; }
    }
}
