namespace CalendarBackend.Application.QueryHandlers
{
    using CalendarBackend.Application.Queries;
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetAllUsersHandler : IRequestHandler<GetAllUsers, IEnumerable<User>>
    {
        public GetAllUsersHandler(UserReadModel model)
        {
            this.Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public UserReadModel Model { get; }

#pragma warning disable CC0061 // Asynchronous method can be terminated with the 'Async' keyword.
        public async Task<IEnumerable<User>> Handle(GetAllUsers message, CancellationToken cancellationToken = default)
#pragma warning restore CC0061 // Asynchronous method can be terminated with the 'Async' keyword.
        {
            return await this.Model.GetEntriesAsync(cancellationToken);
        }
    }
}
