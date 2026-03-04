namespace GcodeRepeater.Gcode.Parsers
{
    internal interface IGcodeFileParser
    {
        public GcodeFileInfo ParseGcodeFile(string filePath);
    }
}