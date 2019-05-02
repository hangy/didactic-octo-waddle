namespace CalendarBackend.Hubs
{
    using CalendarBackend.Domain.Events;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using System.Threading;
    using System.Threading.Tasks;

    [Authorize(Policy = "TeamMember")]
    public class Event : Hub, INotificationHandler<IDomainEvent>
    {
        public Task Handle(IDomainEvent notification, CancellationToken cancellationToken) => this.Clients.All.SendAsync("handle", notification, cancellationToken);
    }
}
