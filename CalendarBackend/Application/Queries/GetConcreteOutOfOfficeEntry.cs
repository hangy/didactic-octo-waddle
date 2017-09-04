namespace CalendarBackend.Application.Queries
{
    using CalendarBackend.Application.Models;
    using MediatR;

    public class GetConcreteOutOfOfficeEntry : IRequest<OutOfOffice>
    {
        public GetConcreteOutOfOfficeEntry(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
    }
}
