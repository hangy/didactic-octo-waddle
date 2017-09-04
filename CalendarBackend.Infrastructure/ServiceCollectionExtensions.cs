namespace CalendarBackend.Infrastructure
{
    using CalendarBackend.Domain.AggregatesModel.OutOfOfficeAggregate;
    using CalendarBackend.Domain.SeedWork;
    using Microsoft.Extensions.DependencyInjection;
    using Raven.Client.Documents;
    using System;

    public static class ServiceCollectionExtensions
    {
        public static void RegisterInfrastructure(this IServiceCollection services, string url)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(sp =>
            {
                return new DocumentStore
                {
                    Urls = new[] { url },
                    Database = "Calendar"
                }.Initialize();
            });

            services.AddScoped(sp => sp.GetRequiredService<IDocumentStore>().OpenAsyncSession());
            services.AddScoped<BackendContext>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<BackendContext>());

            services.AddScoped<IOutOfOfficeRepository, OutOfOfficeRepository>();
        }
    }
}
