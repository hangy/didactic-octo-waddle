namespace CalendarBackend.Infrastructure.EventStore
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEventStore
    {
        Task<IEventReader> GetReaderAsync(CancellationToken cancellationToken = default);

        Task<IEventWriter> GetWriterAsync(CancellationToken cancellationToken = default);
    }
}