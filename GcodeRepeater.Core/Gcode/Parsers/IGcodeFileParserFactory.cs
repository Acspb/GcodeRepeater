namespace GcodeRepeater.Gcode.Parsers
{
    public interface IGcodeFileParserFactory
    {
        public IGcodeFileParser GetParser(string path);
    }
}