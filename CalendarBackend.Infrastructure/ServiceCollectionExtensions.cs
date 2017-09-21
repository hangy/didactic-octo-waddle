namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using CalendarBackend.Infrastructure.EventStore;
    using CalendarBackend.Infrastructure.ReadModel;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ServiceCollectionExtensions
    {
        public static void RegisterInfrastructure(this IServiceCollection services, string path)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IEventStore>(sp => new EventStore.EventStore(path));
            services.AddScoped<IEventStream, EventStream>();

            services.AddScoped<OutOfOfficeReadModel>();

            services.AddScoped<IOutOfOfficeRepository, OutOfOfficeRepository>();
        }
    }
}
