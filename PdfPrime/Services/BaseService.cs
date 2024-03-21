using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PdfPrime.AppSettings;
using Serilog;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfPrime.Services
{
    public interface IBaseService
    {
        public ApplicationOptions Options { get; }
        public CustomCommandLineOptions CommandLineOptions { get; }
        public ILogger<BaseService> Logger { get; }
    }
    public class BaseService : IBaseService
    {
        private readonly ApplicationOptions _options;
        //private readonly IOptions<ApplicationOptions> options;
        private readonly CustomCommandLineOptions _commandLineOptions;

        private readonly ILogger<BaseService> _logger;

        //readonly ApplicationOptions
        public BaseService(ILogger<BaseService> logger, IOptions<ApplicationOptions> options, CustomCommandLineOptions commandLineOptions)
        {
            _options = options.Value;

            Directory.CreateDirectory(_options.GetApplicationDataPath());
            Directory.CreateDirectory(_options.GetApplicationTempPath());

            Log.Logger.Debug("Application Data Path {0}", _options.GetApplicationDataPath());
            Log.Logger.Debug("Application Temp Path {0}", _options.GetApplicationTempPath());

            Log.Logger.Debug("Input appsettings.json {0}", _options.ToJson());

            _logger = logger;
            _commandLineOptions = commandLineOptions;

            _options.ApplicationOutputPath = _commandLineOptions.Output ?? _options.GetApplicationDataPath() ?? _options.GetApplicationTempPath();

            Directory.CreateDirectory(_options.ApplicationOutputPath);

        }

        public ApplicationOptions Options { get => _options; }
        public CustomCommandLineOptions CommandLineOptions { get => _commandLineOptions; }
        public ILogger<BaseService> Logger { get => _logger; }

    }
}
