namespace CalendarBackend.Hubs
{
    using CalendarBackend.Domain.Events;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using System.Threading;
    using System.Threading.Tasks;

    [Authorize(Policy = "TeamMember")]
#pragma warning disable CA1716 // Bezeichner dürfen nicht mit Schlüsselwörtern übereinstimmen
    public class Event : Hub, INotificationHandler<IDomainEvent>
#pragma warning restore CA1716 // Bezeichner dürfen nicht mit Schlüsselwörtern übereinstimmen
    {
        public Task Handle(IDomainEvent notification, CancellationToken cancellationToken) => this.Clients.All.SendAsync("handle", notification, cancellationToken);
    }
}
