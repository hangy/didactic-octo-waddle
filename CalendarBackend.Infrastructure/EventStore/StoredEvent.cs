namespace CalendarBackend.Infrastructure.EventStore
{
    using System;

    public class StoredEvent
    {
        public StoredEvent(string typeName, string json)
        {
            this.TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            this.Json = json ?? throw new ArgumentNullException(nameof(json));
        }

        public string Json { get; }

        public string TypeName { get; }
    }
}
