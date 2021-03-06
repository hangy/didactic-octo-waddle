﻿namespace CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate
{
    using CalendarBackend.Domain.SeedWork;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IOutOfOfficeRepository : IRepository<OutOfOffice>
    {
        Task<OutOfOffice> AddAsync(OutOfOffice outOfOffice, CancellationToken cancellationToken = default);

        Task<OutOfOffice?> GetAsync(Guid outOfOfficeId, CancellationToken cancellationToken = default);

        Task<int> UpdateAsync(OutOfOffice outOfOffice, CancellationToken cancellationToken = default);
    }
}
