namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.SeedWork;
    using Raven.Client.Documents.Session;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class BackendContext : IUnitOfWork
    {
        private bool disposedValue;

        public BackendContext(IAsyncDocumentSession documentStore)
        {
            this.Session = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        public IAsyncDocumentSession Session { get; }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => this.Session.SaveChangesAsync(cancellationToken);

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
                }

                this.disposedValue = true;
            }
        }
    }
}
