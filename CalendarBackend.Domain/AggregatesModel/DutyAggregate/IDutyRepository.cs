namespace CalendarBackend.Domain.AggregatesModel.DutyAggregate
{
    using CalendarBackend.Domain.SeedWork;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDutyRepository : IRepository<Duty>
    {
        Task<Duty> AddAsync(Duty duty, CancellationToken cancellationToken = default);

        Task<Duty> GetAsync(Guid dutyId, CancellationToken cancellationToken = default);

        Task<int> UpdateAsync(Duty duty, CancellationToken cancellationToken = default);
    }
}
