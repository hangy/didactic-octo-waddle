namespace CalendarBackend.Domain.Events
{
    using MediatR;
    using System;

    public interface IIdentifiedEvent : INotification
    {
        Guid Id { get; }
    }
}
