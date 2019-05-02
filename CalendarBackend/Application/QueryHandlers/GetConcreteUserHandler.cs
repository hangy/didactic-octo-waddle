namespace CalendarBackend.Application.QueryHandlers
{
    using CalendarBackend.Application.Queries;
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetConcreteUserHandler : IRequestHandler<GetConcreteUser, User>
    {
        private readonly UserReadModel model;

        public GetConcreteUserHandler(UserReadModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public async Task<User> Handle(GetConcreteUser message, CancellationToken cancellationToken)
        {
            var entries = await this.model.GetEntriesAsync(cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            return entries.SingleOrDefault(e => e.Id == message.Id);
        }
    }
}
