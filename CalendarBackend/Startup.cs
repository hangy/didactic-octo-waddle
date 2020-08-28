namespace CalendarBackend
{
    using CalendarBackend.Authorization;
    using CalendarBackend.Hubs;
    using CalendarBackend.Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using NodaTime.Serialization.JsonNet;
    using NodaTime.Xml;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

#pragma warning disable CA1822 // Mark members as static - This method gets called by the runtime.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore CA1822 // Mark members as static - This method gets called by the runtime.
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<Event>("/event");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(x =>
            {
                var js = new JsonSerializer();
                js.ConfigureForNodaTime(XmlSerializationSettings.DateTimeZoneProvider).WithIsoIntervalConverter();
                return js;
            });

            services.AddSignalR();

            services.AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ConfigureForNodaTime(XmlSerializationSettings.DateTimeZoneProvider).WithIsoIntervalConverter().WithIsoDateIntervalConverter();
                }).AddControllersAsServices();

            services.RegisterInfrastructure(this.Configuration.GetConnectionString("EventStore"));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("TeamMember", policy => policy.AddRequirements(new TeamMemberRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, TeamMemberHandler>();

            ConfigureMediatR(services);
        }

        private static void ConfigureMediatR(IServiceCollection services)
        {
            services.AddScoped<ServiceFactory>(p => t => p.GetRequiredService(t));

            services.AddMediatR(typeof(Startup).Assembly, typeof(Domain.SeedWork.IAggregateRoot).Assembly, typeof(Infrastructure.EventStore.IEventStore).Assembly);
        }
    }
}
