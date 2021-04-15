using System;
using System.Reflection;
using CodeSwifterStarter.Common.Extensions;
using CodeSwifterStarter.Domain;
using CodeSwifterStarter.Persistence;
using CodeSwifterStarter.Persistence.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace CodeSwifterStarter.Web.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var seqKey = Environment.GetEnvironmentVariable("SEQ_KEY");
            seqKey = string.IsNullOrWhiteSpace(seqKey) || seqKey == "NOT_DEFINED" ? null : seqKey;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteForEnvironment(seqKey)
                .CreateLogger();

            var informationalVersion =
                Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "UNKNOWN_VERSION";

            try
            {
                Log.Information($"Starting up CodeSwifterStarter Web.Api, version {informationalVersion}");
                Console.WriteLine($"CodeSwifter CodeSwifterStarter Web.Api, version {informationalVersion}");
                BuildHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
                throw;
                /* If you are facing the issue with certificates, remove all localhost ASP.NET Core certificates from the following stores:
                 
                 Current User > Personal > Certificates
                 Current User > Trusted root certification authorities > Certificates
                 
                 After that run the following command from the command prompt:
                 
                 dotnet dev-certs https --trust

                 Check if certificate is installed using:
                 dotnet dev-certs https --trust

                If you get "No valid certificate found" uninstall, and install dotnet-dev-certs tool and go through above steps again:
                dotnet tool uninstall --global dotnet-dev-certs
                dotnet tool install --global dotnet-dev-certs
                
                */
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildHost(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseConfiguration(new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .Build())
                .ConfigureLogging((hostContext, logging) => { logging.AddSerilog(); })
                .UseStartup<Startup>()
                .ConfigureKestrel(o =>
                {
                    // Prevent server header from leaking what application we are on
                    o.AddServerHeader = false;
                })
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ICodeSwifterStarterDbContext>();
                // ReSharper disable once PossibleNullReferenceException
                context.UpgradeDatabase();

                var seeder = scope.ServiceProvider.GetService<CodeSwifterStarterDbSeeder>();
                // ReSharper disable once PossibleNullReferenceException
                seeder.Seed(SeedType.WebApp);
            }

            return host;
        }
    }
}