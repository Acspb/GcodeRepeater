namespace GcodeRepeater.Gcode
{
    public class GcodeFileInfo
    {
        public int? LayerCount { get; set; }
        public int? StartOfGcodeBodyIndex { get; set; }
        public int? EndOfGcodeBodyIndex { get; set; }
        public int? EndOfGcode { get; set; }
        public int? RepeateIndex { get; set; }
    }
}