namespace GcodeRepeater.Gcode.Parsers
{
    public class OrcaGcodeFileParser : IGcodeFileParser
    {
        public GcodeFileInfo ParseGcodeFile(string filePath)
        {
            var gcodeFileInfo = new GcodeFileInfo();

            using (var streamReader = File.OpenText(filePath))
            {
                string? str;
                var fileLineIndex = 0;
                bool waitingForLastStopPrintingObjectModeOn = false;
                int currentLayerIndex = 0;
                do
                {
                    str = streamReader.ReadLine();
                    if (str == null)
                    {
                        break;
                    }
                    if (!gcodeFileInfo.LayerCount.HasValue)
                    {
                        if (str.StartsWith("; total layer number:", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (int.TryParse(str.Replace("; total layer number:", ""), out int count))
                            {
                                gcodeFileInfo.LayerCount = count;
                            }
                            else
                            {
                                Console.WriteLine("ERROR: unable to parse layers count");
                                break;
                            }
                        }
                    }
                    // ;LAYER:0
                    // ;TIME_ELAPSED: for last layer
                    if (str.StartsWith(";LAYER_CHANGE", StringComparison.InvariantCultureIgnoreCase))
                    {

                        if (!gcodeFileInfo.StartOfGcodeBodyIndex.HasValue)
                        {
                            if (currentLayerIndex == 0)
                            {
                                gcodeFileInfo.StartOfGcodeBodyIndex = fileLineIndex;
                            }
                        }

                        if (gcodeFileInfo.LayerCount.HasValue && gcodeFileInfo.LayerCount == currentLayerIndex + 1)
                        {
                            waitingForLastStopPrintingObjectModeOn = true;
                        }
                        currentLayerIndex++;

                    }
                    if (waitingForLastStopPrintingObjectModeOn)
                    {
                        if (str.StartsWith("; stop printing object", StringComparison.InvariantCultureIgnoreCase))
                        {
                            gcodeFileInfo.EndOfGcodeBodyIndex = fileLineIndex;
                            waitingForLastStopPrintingObjectModeOn = false; // disable this mode because no need to go throuhg this code if we already found TIME_ELAPSED
                        }
                    }

                    if (str.StartsWith(";TYPE:Custom", StringComparison.InvariantCultureIgnoreCase))
                    {
                        gcodeFileInfo.RepeateIndex = fileLineIndex;
                    }

                    if (str.StartsWith("; EXECUTABLE_BLOCK_END", StringComparison.InvariantCultureIgnoreCase))
                    {
                        gcodeFileInfo.EndOfGcode = fileLineIndex;
                    }

                    fileLineIndex++;

                } while (str != null);

            }

            return gcodeFileInfo;
        }
    }
}