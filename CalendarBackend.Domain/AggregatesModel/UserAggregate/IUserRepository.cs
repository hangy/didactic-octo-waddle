namespace CalendarBackend.Domain.AggregatesModel.UserAggregate
{
    using CalendarBackend.Domain.SeedWork;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUserRepository : IRepository<User>
    {
        Task<User> AddAsync(User userId, CancellationToken cancellationToken = default);

        Task<User> GetAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
