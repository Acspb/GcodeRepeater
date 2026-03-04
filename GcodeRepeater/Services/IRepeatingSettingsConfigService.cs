namespace GcodeRepeater.Services
{
    using GcodeRepeater.Gcode;

    internal interface IRepeatingSettingsConfigService
    {
        GcodeRepeatingSettingsInfo GetRepeatingSettingsInfo();
    }
}