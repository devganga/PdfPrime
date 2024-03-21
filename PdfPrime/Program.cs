// See https://aka.ms/new-console-template for more information
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PdfPrime;
using PdfPrime.AppSettings;
using PdfPrime.Services;
using Serilog;

//Console.WriteLine("Hello, World!");

var isValid = Parser.Default.ParseArguments<CustomCommandLineOptions>(args);

var builder = new ConfigurationBuilder();

//if (isValid != null)
//{
//    Console.WriteLine(isValid.Value.Environment);
//}

Configuration.BuildConfig(builder, isValid!.Value.Environment!);


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Build())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger.Information("Application Starting");

Log.Logger.Debug("Input commandline arguments {0}", args);

var host = Host.CreateDefaultBuilder(args)
.ConfigureServices((context, services) =>
{
    services.AddSingleton(typeof(CustomCommandLineOptions), isValid.Value);

    //configure Browser
    Configuration.BrowserConfig(services);

    //App Settings configuration
    Configuration.AppSettingsConfig(context.Configuration, services);

    //Register Services 
    Configuration.DepandancyInjectionConfig(services);
})
.UseSerilog()
.Build();


//Get the Service to Execute
var svc = ActivatorUtilities.CreateInstance<RunService>(host.Services);
await svc.Run();
Log.Logger.Information("Application Ended");

Console.ReadLine();
//terminate console application
Environment.Exit(0);
