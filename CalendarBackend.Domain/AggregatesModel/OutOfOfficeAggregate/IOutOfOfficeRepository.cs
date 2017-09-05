namespace CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate
{
    using CalendarBackend.Domain.SeedWork;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IOutOfOfficeRepository : IRepository<OutOfOffice>
    {
        Task<OutOfOffice> AddAsync(OutOfOffice outOfOffice, CancellationToken cancellationToken = default);

        Task UpdateAsync(OutOfOffice outOfOffice, CancellationToken cancellationToken = default);

        Task<OutOfOffice> GetAsync(string outOfOfficeId, CancellationToken cancellationToken = default);
    }
}
