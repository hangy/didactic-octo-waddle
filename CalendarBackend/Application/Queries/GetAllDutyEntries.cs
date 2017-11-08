namespace CalendarBackend.Application.Queries
{
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using System.Collections.Generic;

    public class GetAllDutyEntries : IRequest<IEnumerable<Duty>>
    {
    }
}
