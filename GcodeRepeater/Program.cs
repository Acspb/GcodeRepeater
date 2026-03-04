using Microsoft.Extensions.DependencyInjection;

namespace GcodeRepeater
{
    using GcodeRepeater.Gcode;
    using GcodeRepeater.Gcode.Parsers;
    using GcodeRepeater.Services;

    internal class Program
    {
        static void Main(string[] args)
        {
            var appArgs = new AppArguments(args);

            var services = new ServiceCollection();
            new Startup(appArgs)
                .ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var gcodeParserFactory = serviceProvider.GetService<IGcodeFileParserFactory>();

            var repeatingSettingService = serviceProvider.GetService<IRepeatingSettingsConfigService>();
            var repeatingSettingsInfo = repeatingSettingService.GetRepeatingSettingsInfo();
            
            DoRepeate(repeatingSettingsInfo, gcodeParserFactory);
        }

        private static void DoRepeate(GcodeRepeatingSettingsInfo gcodeRepeatingSettingsInfo, IGcodeFileParserFactory gcodeParserFactory)
        {
            if (gcodeRepeatingSettingsInfo == null)
            {
                throw new ArgumentException("Setting Info is null", nameof(gcodeRepeatingSettingsInfo));
            }
            if (string.IsNullOrEmpty( gcodeRepeatingSettingsInfo.Path))
            {
                throw new ArgumentException("File path is not specified", nameof(gcodeRepeatingSettingsInfo.Path));
            }

            //var gcodeFileInfo = new CuraGcodeFileParser()
            
            var gcodeFileInfo = gcodeParserFactory.GetParser(gcodeRepeatingSettingsInfo.Path)
                .ParseGcodeFile(gcodeRepeatingSettingsInfo.Path);
            new GcodeRepeaterManager()
                .Repeate(gcodeRepeatingSettingsInfo, gcodeFileInfo);
        }
        /*
        private static void DoRepeate(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Help: GcodeRepeater.exe path [optional]repeateCount");
                Console.WriteLine("Examle 1: GcodeRepeater.exe \"MyFile.gcode\"");
                Console.WriteLine("\tResult: new file generated with default repeate Count = 2");

                Console.WriteLine("Example 2: GcodeRepeater.exe \\\"MyFile.gcode\\\"\" 20");
                Console.WriteLine("\tResult: new file generated with repeate Count = 20");

                return;
            }

            var filePath = args[0];
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found '{filePath}'");
                return;
            }
            int count = 10;

            if (args.Length == 2)
            {
                if (!int.TryParse(args[1], out count))
                {
                    Console.WriteLine("Invalida parameter: second parameter should be integer count of repeating");
                    return;
                }
            }

            var parser = new GcodeFileParser();
            var gcodeFileInfo = parser.ParseGcodeFile(filePath);
            var repeater = new GcodeRepeaterManager();
            repeater.Repeate(gcodeFileInfo, count);

            Console.WriteLine("Done");
        }*/
    }
}