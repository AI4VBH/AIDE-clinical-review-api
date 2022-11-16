using Aide.ClinicalReview.Common.Interfaces;
using Aide.ClinicalReview.Common.Services;
using Aide.ClinicalReview.Contracts.Messages;
using Aide.ClinicalReview.Database.Configuration;
using Aide.ClinicalReview.Database.Interfaces;
using Aide.ClinicalReview.Database.Repository;
using Aide.ClinicalReview.Service.Handler;
using Aide.ClinicalReview.Service.Services;
using Aide.ClinicalReview.Service.Services.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monai.Deploy.Messaging;
using Monai.Deploy.Messaging.Configuration;
using Monai.Deploy.Storage;
using Monai.Deploy.Storage.Configuration;
using MongoDB.Driver;
using NLog;
using NLog.LayoutRenderers;
using NLog.Web;
using System.IO.Abstractions;
using System.Reflection;

namespace Aide.ClinicalReview.Service
{
    public sealed class Program
    {
        private Program()
        { }

        private static void Main(string[] args)
        {
            var version = typeof(Program).Assembly;
            var assemblyVersionNumber = version.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "0.0.1";

            var logger = ConfigureNLog(assemblyVersionNumber);
            logger.Info($"Initializing {ClincalReviewService.ServiceName} v{assemblyVersionNumber}");

            var host = CreateHostBuilder(args).Build();
            host.Run();
            logger.Info($"{ClincalReviewService.ServiceName} shutting down.");

            NLog.LogManager.Shutdown();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var env = builderContext.HostingEnvironment;
                    config
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging((builderContext, configureLogging) =>
                {
                    configureLogging.ClearProviders();
                    configureLogging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    ConfigureServices(hostContext, services);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseStartup<Startup>();
                })
                .UseNLog();

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddMonaiDeployMessageBrokerPublisherService(hostContext.Configuration.GetSection("AideClinicalReviewService:messaging:publisherServiceAssemblyName").Value);
            services.AddMonaiDeployMessageBrokerSubscriberService(hostContext.Configuration.GetSection("AideClinicalReviewService:messaging:subscriberServiceAssemblyName").Value);

            services.AddOptions<MessageBrokerServiceConfiguration>().Bind(hostContext.Configuration.GetSection("AideClinicalReviewService:messaging"));
            services.AddOptions<StorageServiceConfiguration>().Bind(hostContext.Configuration.GetSection("AideClinicalReviewService:storage"));

            services.AddSingleton<IClinicalReviewRepository, ClinicalReviewRepository>();
            services.AddSingleton<ICallBackHandler<AideClinicalReviewRequestMessage>, ReviewRequestCallBackHandler>();
            services.AddHttpClient();

            services.AddSingleton<IClinicalReviewRepository, ClinicalReviewRepository>();
            services.AddTransient<IClinicalReviewService, ClinicalReviewService>();
            services.AddSingleton<ICallBackHandler<AideClinicalReviewRequestMessage>, ReviewRequestCallBackHandler>();
            services.AddHttpClient();

            services.AddHttpContextAccessor();
            services.AddSingleton<IUriService>(p =>
            {
                var accessor = p.GetRequiredService<IHttpContextAccessor>();
                var request = accessor?.HttpContext?.Request;
                var uri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent());
                var newUri = new Uri(uri);
                return new UriService(newUri);
            });

            // Mongo DB (Workflow Manager)
            services.Configure<AideClinicalReviewDatabaseSettings>(hostContext.Configuration.GetSection("AideClinicalReviewDatabase"));
            services.AddSingleton<IMongoClient, MongoClient>(s => new MongoClient(hostContext.Configuration["AideClinicalReviewDatabase:ConnectionString"]));

            services.AddTransient<IFileSystem, FileSystem>();

            services.AddMonaiDeployStorageService(hostContext.Configuration.GetSection("AideClinicalReviewService:storage:serviceAssemblyName").Value);
            services.AddSingleton<DicomService>();

            services.AddSingleton<ClincalReviewService>();
            services.AddHostedService(p => p.GetRequiredService<ClincalReviewService>());
        }

        private static Logger ConfigureNLog(string assemblyVersionNumber)
        {
            LayoutRenderer.Register("servicename", logEvent => typeof(Program).Namespace);
            LayoutRenderer.Register("serviceversion", logEvent => assemblyVersionNumber);
            LayoutRenderer.Register("machinename", logEvent => Environment.MachineName);

            return LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        }
    }
}
