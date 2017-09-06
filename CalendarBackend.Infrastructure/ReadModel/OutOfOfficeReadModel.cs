namespace CalendarBackend.Infrastructure.ReadModel
{
    using EventStore.ClientAPI;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OutOfOfficeReadModel : IDisposable
    {
        private bool disposedValue;

        private readonly List<object> entries = new List<object>();

        public OutOfOfficeReadModel(Func<Task<IEventStoreConnection>> connectionFactory)
        {
            this.ConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this.Subscribe();
        }

        public Func<Task<IEventStoreConnection>> ConnectionFactory { get; }

        public IReadOnlyList<object> Entries { get => this.entries; }

        public EventStoreCatchUpSubscription Subscription { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.Subscription?.Stop();
                }

                this.disposedValue = true;
            }
        }

        private void Dropped(EventStoreCatchUpSubscription sub, SubscriptionDropReason reason, Exception ex)
        {
            this.Subscribe();
        }

        private async Task GotEventAsync(EventStoreCatchUpSubscription sub, ResolvedEvent evt)
        {
            if (!evt.Event.IsJson)
            {
                return;
            }

            this.entries.Add(evt.Event);
        }

        private void Subscribe()
        {
            this.Subscription = this.ConnectionFactory().Result.SubscribeToStreamFrom("OutOfOffice", null, CatchUpSubscriptionSettings.Default, this.GotEventAsync, subscriptionDropped: this.Dropped);
        }
    }
}
