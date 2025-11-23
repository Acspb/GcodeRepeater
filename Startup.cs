using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GcodeRepeater
{
    using GcodeRepeater.Gcode.Parsers;
    using GcodeRepeater.Services;

    public class Startup
    {
        private IConfigurationRoot Configuration { get; }

        public Startup(AppArguments appArgs)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appArgs.AppSettingsFileName);

            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging()
                .AddSingleton<IConfigurationRoot>(Configuration)
                .AddSingleton<IGcodeFileParserFactory, GcodeFileParserFactory>()
                .AddSingleton<IRepeatingSettingsConfigService, RepeatingSettingsConfigService>();
        }
    }
}