namespace CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate
{
    using CalendarBackend.Domain.SeedWork;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IOutOfOfficeRepository : IRepository<OutOfOffice>
    {
        Task<OutOfOffice> AddAsync(OutOfOffice outOfOffice, CancellationToken cancellationToken = default);

        void Update(OutOfOffice outOfOffice);

        Task<OutOfOffice> GetAsync(string outOfOfficeId, CancellationToken cancellationToken = default);
    }
}
