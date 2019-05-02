namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.AggregatesModel.UserAggregate;
    using CalendarBackend.Infrastructure.EventStore;
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Threading;
    using System.Threading.Tasks;

    public class UserRepository : IUserRepository
    {
        private readonly IEventStream eventStream;

        private readonly ReadModel.UserReadModel readModel;

        public UserRepository(IEventStream eventStream, ReadModel.UserReadModel readModel)
        {
            this.eventStream = eventStream ?? throw new ArgumentNullException(nameof(eventStream));
            this.readModel = readModel ?? throw new ArgumentNullException(nameof(readModel));
        }

        public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await this.eventStream.WriteEventsAsync(user.DomainEvents, cancellationToken).ConfigureAwait(false);
            return user;
        }

        public async Task<User?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var entries = await this.readModel.GetEntriesAsync(cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            var entry = entries.SingleOrDefault(e => e.Id == userId);
            return entry == null ? null : new User(entry.Id, entry.UserName, entry.DisplayName, new MailAddress(entry.MailAddress), new UserColor(entry.Color), entry.DomainEvents);
        }
    }
}