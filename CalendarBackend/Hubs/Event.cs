namespace CalendarBackend.Hubs
{
    using CalendarBackend.Domain.Events;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    [Authorize(Policy = "TeamMember")]
    public class Event : Hub, IAsyncNotificationHandler<IDomainEvent>
    {
        public Task Handle(IDomainEvent notification) => this.Clients.All.InvokeAsync("handle", notification);
    }
}
