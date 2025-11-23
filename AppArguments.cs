namespace GcodeRepeater
{
    public class AppArguments
    {
        private readonly string DefaultAppSettingsName = "appsettings.json";
        public string AppSettingsFileName { get; set; }

        public AppArguments(string[] args)
        {
            if (args.Length > 1)
            {
                throw new ArgumentException("Only settings.json file path can be accepted as an argument");
            }

            if (args.Length == 1)
            {

                this.AppSettingsFileName = args[0];
            }
            else
            {
                this.AppSettingsFileName = DefaultAppSettingsName;
            }
        }

    }
}