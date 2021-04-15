using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using CodeSwifterStarter.Application.Extensions;
using CodeSwifterStarter.Application.Infrastructure;
using CodeSwifterStarter.Application.Infrastructure.AutoMapper;
using CodeSwifterStarter.Application.Interfaces;
using CodeSwifterStarter.Application.Tests.Fakes.Services;
using CodeSwifterStarter.Common.Interfaces;
using CodeSwifterStarter.Common.Models;
using CodeSwifterStarter.Domain;
using CodeSwifterStarter.Infrastructure;
using CodeSwifterStarter.Persistence;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CodeSwifterStarter.Application.Tests
{
    public class Startup
    {
        private ServerConfiguration _serverConfiguration;

        public void ConfigureServices(IServiceCollection services)
        {
            // Add Singletons
            services.AddSingleton<IEnvironmentInformationProvider>(new FakeEnvironmentInformationProvider());

            // Add AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile).GetTypeInfo().Assembly);

            // Register all query managers
            services.AddScopedForClassesEndingWith(typeof(AutoMapperProfile).GetTypeInfo().Assembly, "QueryManager");


            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(typeof(AutoMapperProfile).GetTypeInfo().Assembly);

            // Add database context
            services.AddDbContext<CodeSwifterStarterDbContext>(options =>
                {
                    options.UseInMemoryDatabase("CodeSwifterStarterDB-" + Guid.NewGuid());
                    options.UseLazyLoadingProxies();
                    options.ConfigureWarnings(warningOptions =>
                    {
                        warningOptions.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                    });
                }
            );

            services.AddScoped(service =>
                (ICodeSwifterStarterDbContext) service.GetService(typeof(CodeSwifterStarterDbContext)));
            services.AddScoped<CodeSwifterStarterDbSeeder>();

            var fileName = "appsettings.json";
            _serverConfiguration = GetConfigurationFromJson(fileName);
            services.AddSingleton(_serverConfiguration);


            // Register framework (some of them should be fake) services
            services.AddScoped<IAuthenticatedUserService, FakeAuthenticatedUserService>();
            services.AddTransient<INotificationService, FakeNotificationService>();
            services.AddTransient<ICrudWarningService, FakeCrudWarningService>();
            services.AddTransient<IDateTime, MachineDateTime>();
        }

        private ServerConfiguration GetConfigurationFromJson(string fileName)
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            var uri = new UriBuilder(codeBase ?? string.Empty);
            var path = Uri.UnescapeDataString(uri.Path);

            var file = Path.Combine(Path.GetDirectoryName(path), fileName);

            return JsonConvert.DeserializeObject<ServerConfiguration>(File.ReadAllText(file));
        }
    }
}