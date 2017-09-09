namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using CalendarBackend.Infrastructure.ReadModel;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ServiceCollectionExtensions
    {
        public static void RegisterInfrastructure(this IServiceCollection services, string url)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(typeof(IEventStream<>), typeof(EventStream<>));

            services.AddSingleton<OutOfOfficeReadModel>();

            services.AddScoped<IOutOfOfficeRepository, OutOfOfficeRepository>();
        }
    }
}
