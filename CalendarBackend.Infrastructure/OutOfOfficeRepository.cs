namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using CalendarBackend.Domain.SeedWork;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class OutOfOfficeRepository : IOutOfOfficeRepository
    {
        public OutOfOfficeRepository(BackendContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => this.Context;

        private BackendContext Context { get; }

        public async Task<OutOfOffice> AddAsync(OutOfOffice outOfOffice, CancellationToken cancellationToken = default)
        {
            if (outOfOffice == null)
            {
                throw new ArgumentNullException(nameof(outOfOffice));
            }

            await this.Context.Session.StoreAsync(outOfOffice, cancellationToken).ConfigureAwait(false);
            return outOfOffice;
        }

        public Task<OutOfOffice> GetAsync(string outOfOfficeId, CancellationToken cancellationToken = default) => this.Context.Session.LoadAsync<OutOfOffice>(outOfOfficeId, cancellationToken);

        public Task UpdateAsync(OutOfOffice outOfOffice, CancellationToken cancellationToken = default)
        {
            // Noop? Should store when savechanges is done
            return Task.CompletedTask;
        }
    }
}
