namespace CalendarBackend.Application.Queries
{
    using CalendarBackend.Application.Models;
    using MediatR;

    public class GetConcreteOutOfOfficeEntry : IRequest<OutOfOffice>
    {
        public GetConcreteOutOfOfficeEntry(int id)
        {
            this.Id = id;
        }

        public int Id { get; }
    }
}
