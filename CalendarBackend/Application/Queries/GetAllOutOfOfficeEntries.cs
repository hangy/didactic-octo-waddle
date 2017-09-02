namespace CalendarBackend.Application.Queries
{
    using CalendarBackend.Application.Models;
    using MediatR;
    using System.Collections.Generic;

    public class GetAllOutOfOfficeEntries : IRequest<IEnumerable<OutOfOffice>>
    {
    }
}
