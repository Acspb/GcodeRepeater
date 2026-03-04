namespace GcodeRepeater.Gcode.Parsers
{
    public interface IGcodeFileParser
    {
        public GcodeFileInfo ParseGcodeFile(string filePath);
    }
}