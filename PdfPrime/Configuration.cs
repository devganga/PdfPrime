using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PdfPrime.AppSettings;
using PdfPrime.Services;

namespace PdfPrime
{
    public static class Configuration
    {       

        /// <summary>
        /// Configure appsettings json from the file
        /// </summary>
        /// <param name="builder"></param>
        public static void BuildConfig(this IConfigurationBuilder builder, string env)
        {
            //Console.WriteLine($"Environment : {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production"}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            //.AddCommandLine(args)                
        }

        /// <summary>
        /// Configure Browser to load the application
        /// </summary>
        /// <param name="services"></param>
        public static void BrowserConfig(this IServiceCollection services)
        {
            #region "Selinum Browser Inject"

            //Inject Edge Browser Selinium
            //services.AddTransient<IWebDriver, EdgeDriver>();
            //Inject Chrome Browser Selinium 
            //services.AddTransient<IWebDriver, ChromeDriver>();
            //Inject Safari Browser Selinium 
            //services.AddTransient<IWebDriver, SafariDriver>();
            //Inject firefox Browser Selinium 
            //services.AddTransient<IWebDriver, FirefoxDriver>();
            //services.AddSingleton<IBrowserDriver, BrowserDriver>();
            //services.AddSingleton<IDriverFixture, DriverFixture>();


            #endregion
        }

        /// <summary>
        /// Configure the appSettings options from the appsettings json
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="services"></param>
        public static void AppSettingsConfig(this IConfiguration hostBuilder, IServiceCollection services)
        {
            //configure App Config read from appsettings
            //services.Configure<BrowserOptions>(hostBuilder.GetSection(nameof(BrowserOptions)));
            //services.Configure<AdrenalinOptions>(hostBuilder.GetSection(nameof(AdrenalinOptions)));
            //services.Configure<SampleOptions>(hostBuilder.GetSection(nameof(SampleOptions)));
            //services.Configure<EndpointOptions>(hostBuilder.GetSection(nameof(EndpointOptions)));
            services.Configure<ApplicationOptions>(hostBuilder.GetSection(nameof(ApplicationOptions)));

            //services.ConfigureOptions(SampleOptions);
        }

        public static void DepandancyInjectionConfig(this IServiceCollection services)
        {
            //Register Api Http Client
            //services.AddTransient(typeof(IHttpClientHelper<>), typeof(HttpClientHelper<>));

            //services.AddSingleton<IHttpClientService, HttpClientService>();
            //services.AddSingleton<IApplicationConfigService, ApplicationConfigService>();

            //services.AddTransient<ITempFileService, TempFileService>();

            //services.AddTransient<IFileEncrypt, FileEncrypt>();
            //services.AddTransient<IFileSecureService, FileSecureService>();
            //services.AddTransient<IHtml2PdfService, Html2PdfService>();

            ////Register Services
            ////services.AddTransient<IRunService, AdrenalinService>();
            ////services.AddTransient<IRunService, FourSeasonService>();
            services.AddSingleton<IBaseService, BaseService>();

            services.AddTransient<IRunService, RunService>();

            services.AddTransient<IFileSecureService, FileSecureService>();
            services.AddTransient<ITempFileService, TempFileService>();
            //services.AddTransient<IRunService, encr>();

        }
    }
}
