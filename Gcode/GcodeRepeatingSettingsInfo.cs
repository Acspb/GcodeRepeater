namespace GcodeRepeater.Gcode
{
    internal class GcodeRepeatingSettingsInfo
    {
        private const string DefaultRepeateGcode = @"; do nothing";
        private const string DefaultPreRepeateGcode = "G92 E0 ; reset extruder offset"; // reset extruder offset

        private string? _repeatingGcode;
        private string? _preRepeatingGcode;

        public string? Path { get; set; }
        // TODO: move to config
        /*private const string DefaultRepeateGcode_0 = @"G1 Z80 F2000  ; drop model
G1 Y80 F5000 ; drop model
G1 Y180 F5000 ; drop model
G1 Z50 F2000  ; drop model
G1 Y20 F5000 ; drop model";
        */
        /*
        private const string DefaultRepeateGcode = @"
G1 Y180 F5000 ; drop model
G1 X100 F5000 ; drop model

G1 Z80 F2000  ; drop model
G1 Y80 F5000 ; drop model
G1 Y180 F5000 ; drop model
G1 Z50 F2000  ; drop model
G1 Y20 F5000 ; drop model";
        /*
        /*
        private const string DefaultRepeateGcode_Nose = @"
G1 Y180 F5000 ; drop model
G1 X100 F5000 ; drop model
G1 Z10 F2000 ; drop model
G1 Y20 F5000 ; drop model";*/
        public string RepeateGcode
        {
            get
            {
                return this._repeatingGcode ?? DefaultRepeateGcode;
            }
            set
            {
                this._repeatingGcode = value;
            }
        }
        public string PreRepeatingGcode
        {
            get
            {
                return this._preRepeatingGcode ?? DefaultPreRepeateGcode;
            }
            set
            {
                this._preRepeatingGcode = value;
            }
        }
        public string[] SkipCommands { get; internal set; }
        public string? TargetPath { get; internal set; }
        public int RepeateCount { get; internal set; }
        public GcodeRepeatingSettingsInfo()
        {
            SkipCommands = new string[0];
        }
    }
}