using System.Text.RegularExpressions;

namespace GcodeRepeater.Gcode
{
    internal class GcodeRepeaterManager
    {
        public void Repeate(GcodeRepeatingSettingsInfo gcodeRepeatingSettingsInfo, GcodeFileInfo gcodeFileInfo)
        {
            if (gcodeFileInfo == null)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo));
            }
            if (string.IsNullOrEmpty(gcodeRepeatingSettingsInfo.Path))
            {
                throw new ArgumentException(nameof(gcodeRepeatingSettingsInfo.Path));
            }
            string path = gcodeRepeatingSettingsInfo.Path;

            using (var fileTarget = File.CreateText(this.GetTargetFilePath(gcodeRepeatingSettingsInfo)))
            {
                WriteInitialGcode(path, gcodeFileInfo, fileTarget);
                for (var i = 0; i < gcodeRepeatingSettingsInfo.RepeateCount; i++)
                {
                    WriteBodyGcode(path, gcodeFileInfo, fileTarget);
                    WriteBeforeRepeateGcode(path, gcodeRepeatingSettingsInfo, gcodeFileInfo, fileTarget);
                    WriteDropGcode(gcodeRepeatingSettingsInfo, fileTarget);
                }
                WriteFinalizeGcode(path, gcodeFileInfo, fileTarget);
            }

        }

        private string GetTargetFilePath(GcodeRepeatingSettingsInfo gcodeRepeatingSettingsInfo)
        {
            return gcodeRepeatingSettingsInfo.TargetPath ?? GenerateFileName(gcodeRepeatingSettingsInfo.Path);
        }

        private void WriteInitialGcode(string path, GcodeFileInfo gcodeFileInfo, StreamWriter fileTarget)
        {
            if (gcodeFileInfo == null)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }

            if (!gcodeFileInfo.StartOfGcodeBodyIndex.HasValue)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo.StartOfGcodeBodyIndex));
            }

            using (var fileOriginal = File.OpenText(path))
            {
                WriteFromTo(fileOriginal, fileTarget, 0, gcodeFileInfo.StartOfGcodeBodyIndex.Value);
            }
        }

        private void WriteBodyGcode(string path, GcodeFileInfo gcodeFileInfo, StreamWriter fileTarget)
        {
            if (gcodeFileInfo == null)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }

            if (!gcodeFileInfo.StartOfGcodeBodyIndex.HasValue)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo.StartOfGcodeBodyIndex));
            }
            if (!gcodeFileInfo.EndOfGcodeBodyIndex.HasValue)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo.EndOfGcodeBodyIndex));
            }
            using (var fileOriginal = File.OpenText(path))
            {
                WriteFromTo(fileOriginal, fileTarget, gcodeFileInfo.StartOfGcodeBodyIndex.Value, gcodeFileInfo.EndOfGcodeBodyIndex.Value);
            }
        }

        private void WriteBeforeRepeateGcode(string path, GcodeRepeatingSettingsInfo gcodeRepeatingSettingsInfo, GcodeFileInfo gcodeFileInfo, StreamWriter fileTarget)
        {
            if (gcodeFileInfo == null)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }

            if (!gcodeFileInfo.EndOfGcodeBodyIndex.HasValue)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo.EndOfGcodeBodyIndex));
            }
            if (!gcodeFileInfo.RepeateIndex.HasValue)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo.RepeateIndex));
            }

            //var skipCommands = skipHeatBedTurnoff ? new[] { "M140 S0" } : null;
            using (var fileOriginal = File.OpenText(path))
            {
                WriteFromTo(
                    fileOriginal,
                    fileTarget,
                    gcodeFileInfo.EndOfGcodeBodyIndex.Value,
                    gcodeFileInfo.RepeateIndex.Value,
                    gcodeRepeatingSettingsInfo.SkipCommands);
            }
        }

        private void WriteDropGcode(GcodeRepeatingSettingsInfo gcodeFileInfo, StreamWriter fileTarget)
        {
            fileTarget.WriteLine(gcodeFileInfo.PreRepeatingGcode);
            fileTarget.Write(gcodeFileInfo.RepeateGcode);
            fileTarget.WriteLine();
        }

        private void WriteFinalizeGcode(string path, GcodeFileInfo gcodeFileInfo, StreamWriter fileTarget)
        {
            if (gcodeFileInfo == null)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }

            if (!gcodeFileInfo.RepeateIndex.HasValue)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo.RepeateIndex));
            }
            if (!gcodeFileInfo.EndOfGcode.HasValue)
            {
                throw new ArgumentNullException(nameof(gcodeFileInfo.EndOfGcode));
            }
            using (var fileOriginal = File.OpenText(path))
            {

                WriteFromTo(fileOriginal, fileTarget, gcodeFileInfo.RepeateIndex.Value, gcodeFileInfo.EndOfGcode.Value);
            }
        }

        private void WriteFromTo(StreamReader fileOriginal, StreamWriter fileTarget, int startIndex, int endIndex, string[]? skipCommands = null)
        {
            string? str;
            int lineIndex = 0;
            do
            {
                str = fileOriginal.ReadLine();
                if (str == null)
                {
                    break;
                }
                if (skipCommands != null && skipCommands.Any(c => c.Equals(str)))
                {
                    lineIndex++;
                    continue;
                }
                if (lineIndex >= startIndex)
                {
                    if (lineIndex < endIndex)
                    {
                        fileTarget.WriteLine(str);
                    }
                    else
                    {
                        break;
                    }
                }

                lineIndex++;
            }
            while (str != null);
        }
        private string GenerateFileName(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("File path is not specified", nameof(path));
            }
            var generatedFilePostfix = $"_{DateTime.Now.ToString("yyyyMMddHHmmss", new System.Globalization.CultureInfo("en-US"))}.gcode";
            return new Regex(".gcode$").Replace(path, generatedFilePostfix);
        }
    }
}
