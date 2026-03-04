namespace GcodeRepeater.Gcode.Parsers
{
    internal class CuraGcodeFileParser : IGcodeFileParser
    {
        public GcodeFileInfo ParseGcodeFile(string filePath)
        {
            var gcodeFileInfo = new GcodeFileInfo();

            using (var streamReader = File.OpenText(filePath))
            {
                string? str;
                var fileLineIndex = 0;
                bool waitingForLastTimeElapsedModeOn = false;
                do
                {
                    str = streamReader.ReadLine();
                    int currentLayerIndex;
                    if (str == null)
                    {
                        break;
                    }
                    if (!gcodeFileInfo.LayerCount.HasValue)
                    {
                        // ;LAYER_COUNT:
                        if (str.StartsWith(";LAYER_COUNT:", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (int.TryParse(str.Replace(";LAYER_COUNT:", ""), out int count))
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
                    if (str.StartsWith(";LAYER:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (int.TryParse(str.Replace(";LAYER:", string.Empty), out int layerIndex))
                        {
                            currentLayerIndex = layerIndex;

                            if (!gcodeFileInfo.StartOfGcodeBodyIndex.HasValue)
                            {
                                if (currentLayerIndex == 0)
                                {
                                    gcodeFileInfo.StartOfGcodeBodyIndex = fileLineIndex;
                                }
                            }

                            if (gcodeFileInfo.LayerCount.HasValue && gcodeFileInfo.LayerCount == currentLayerIndex + 1)
                            {
                                waitingForLastTimeElapsedModeOn = true;
                            }
                        }

                    }
                    if (waitingForLastTimeElapsedModeOn)
                    {
                        if (str.StartsWith(";TIME_ELAPSED:", StringComparison.InvariantCultureIgnoreCase))
                        {
                            gcodeFileInfo.EndOfGcodeBodyIndex = fileLineIndex;
                            waitingForLastTimeElapsedModeOn = false; // disable this mode because no need to go throuhg this code if we already found TIME_ELAPSED
                        }
                    }

                    if (str.StartsWith(";[REPEATE HERE]", StringComparison.InvariantCultureIgnoreCase))
                    {
                        gcodeFileInfo.RepeateIndex = fileLineIndex;
                    }

                    if (str.StartsWith(";End of Gcode", StringComparison.InvariantCultureIgnoreCase))
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
