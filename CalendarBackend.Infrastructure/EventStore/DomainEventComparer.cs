namespace CalendarBackend.Infrastructure.EventStore
{
    using CalendarBackend.Domain.Events;
    using System.Collections.Generic;

    public class DomainEventComparer : IEqualityComparer<IDomainEvent>
    {
        public bool Equals(IDomainEvent x, IDomainEvent y) => x?.Id == y?.Id;

        public int GetHashCode(IDomainEvent obj) => obj?.Id.GetHashCode() ?? 0;
    }
}
