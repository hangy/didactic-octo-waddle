﻿namespace CalendarBackend
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using System.Threading.Tasks;

    public class Program
    {
        public static Task Main(string[] args)
        {
            return BuildWebHost(args).RunAsync();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
