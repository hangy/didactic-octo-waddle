namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.AggregatesModel.DutyAggregate;
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using CalendarBackend.Domain.AggregatesModel.UserAggregate;
    using CalendarBackend.Infrastructure.EventStore;
    using CalendarBackend.Infrastructure.ReadModel;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System;

    public static class ServiceCollectionExtensions
    {
        public static void RegisterInfrastructure(this IServiceCollection services, string path)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IEventStore>(sp => new EventStore.EventStore(sp.GetRequiredService<JsonSerializer>(), path));
            services.AddScoped<IEventStream, EventStream>();

            services.AddScoped<UserReadModel>();
            services.AddScoped<OutOfOfficeReadModel>();
            services.AddScoped<DutyReadModel>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOutOfOfficeRepository, OutOfOfficeRepository>();
            services.AddScoped<IDutyRepository, DutyRepository>();
        }
    }
}
