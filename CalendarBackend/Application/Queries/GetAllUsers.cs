namespace CalendarBackend.Application.Queries
{
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using System.Collections.Generic;

    public class GetAllUsers : IRequest<IEnumerable<User>>
    {
    }
}
