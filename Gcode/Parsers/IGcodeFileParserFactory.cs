namespace GcodeRepeater.Gcode.Parsers
{
    internal interface IGcodeFileParserFactory
    {
        internal IGcodeFileParser GetParser(string path);
    }
}