using Microsoft.Extensions.Configuration;

namespace GcodeRepeater.Services
{
    using GcodeRepeater.Gcode;

    internal class RepeatingSettingsConfigService : IRepeatingSettingsConfigService
    {
        private readonly GcodeRepeatingSettingsInfo _repeatingInfo;

        public RepeatingSettingsConfigService(IConfigurationRoot config)
        {
            var originalGcodePath = config["RepeatingSettings:OriginalGcodePath"];
            var targetGcodePath = config["RepeatingSettings:TargetGcodePath"];
            var repeateCount = Convert.ToInt32(config["RepeatingSettings:RepeateCount"]);

            var preRepeatingGcode = config.GetSection("RepeatingSettings:PreRepeatingGcodeLines").Get<string[]>();
            var repeatingGcode = config.GetSection("RepeatingSettings:RepeatingGcodeLines").Get<string[]>();
            var skipCommands = config.GetSection("RepeatingSettings:SkipCommands").Get<string[]>();

            this._repeatingInfo = new GcodeRepeatingSettingsInfo
            {
                Path = originalGcodePath,
                RepeateCount = repeateCount,
                TargetPath = targetGcodePath,
                PreRepeatingGcode = string.Join(Environment.NewLine, preRepeatingGcode ?? Array.Empty<string>()),
                RepeateGcode = string.Join(Environment.NewLine, repeatingGcode ?? Array.Empty<string>()),
                SkipCommands = skipCommands ?? Array.Empty<string>()
            };
        }

        public GcodeRepeatingSettingsInfo GetRepeatingSettingsInfo()
        {
            return this._repeatingInfo;
        }
    }
}