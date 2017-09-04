namespace CalendarBackend.Application.Queries
{
    using CalendarBackend.Application.Models;
    using MediatR;

    public class GetConcreteUser : IRequest<User>
    {
        public GetConcreteUser(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
    }
}
