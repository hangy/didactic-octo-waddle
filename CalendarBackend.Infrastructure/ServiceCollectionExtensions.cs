namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using CalendarBackend.Domain.SeedWork;
    using CalendarBackend.Infrastructure.ReadModel;
    using EventStore.ClientAPI;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public static class ServiceCollectionExtensions
    {
        public static void RegisterInfrastructure(this IServiceCollection services, string url)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<Func<Task<IEventStoreConnection>>>(async () =>
            {
                var store = EventStoreConnection.Create(url);
                store.AuthenticationFailed += (s, e) => Console.WriteLine(e);
                store.Closed += (s, e) => Console.WriteLine(e);
                store.Connected += (s, e) => Console.WriteLine(e);
                store.Disconnected += (s, e) => Console.WriteLine(e);
                store.ErrorOccurred += (s, e) => Console.WriteLine(e);
                store.Reconnecting += (s, e) => Console.WriteLine(e);
                await store.ConnectAsync();
                return store;
            });

            services.AddSingleton<OutOfOfficeReadModel>();

            services.AddScoped<IOutOfOfficeRepository, OutOfOfficeRepository>();
        }
    }
}
