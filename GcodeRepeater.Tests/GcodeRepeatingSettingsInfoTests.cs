using Microsoft.VisualStudio.TestTools.UnitTesting;
using GcodeRepeater.Gcode;

namespace GcodeRepeater.Tests;

[TestClass]
public class GcodeRepeatingSettingsInfoTests
{
    [TestMethod]
    public void GcodeRepeatingSettingsInfo_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var settingsInfo = new GcodeRepeatingSettingsInfo();

        // Assert
        Assert.IsNull(settingsInfo.Path);
        Assert.AreEqual("G92 E0 ; reset extruder offset", settingsInfo.PreRepeatingGcode);
        Assert.AreEqual("; do nothing", settingsInfo.RepeateGcode);
        Assert.IsNotNull(settingsInfo.SkipCommands);
        Assert.AreEqual(0, settingsInfo.SkipCommands.Length);
        Assert.IsNull(settingsInfo.TargetPath);
        Assert.AreEqual(0, settingsInfo.RepeateCount);
    }

    [TestMethod]
    public void GcodeRepeatingSettingsInfo_SetProperties_ValuesAreCorrect()
    {
        // Arrange
        var settingsInfo = new GcodeRepeatingSettingsInfo();
        
        // Act
        settingsInfo.Path = "test.gcode";
        settingsInfo.PreRepeatingGcode = "G92 E1";
        settingsInfo.RepeateGcode = "G1 X100";
        settingsInfo.SkipCommands = new[] { "M106", "M107" };
        settingsInfo.TargetPath = "output.gcode";
        settingsInfo.RepeateCount = 5;

        // Assert
        Assert.AreEqual("test.gcode", settingsInfo.Path);
        Assert.AreEqual("G92 E1", settingsInfo.PreRepeatingGcode);
        Assert.AreEqual("G1 X100", settingsInfo.RepeateGcode);
        CollectionAssert.AreEqual(new[] { "M106", "M107" }, settingsInfo.SkipCommands);
        Assert.AreEqual("output.gcode", settingsInfo.TargetPath);
        Assert.AreEqual(5, settingsInfo.RepeateCount);
    }

    [TestMethod]
    public void GcodeRepeatingSettingsInfo_CustomPreRepeatingGcode_SetsValue()
    {
        // Arrange
        var settingsInfo = new GcodeRepeatingSettingsInfo();
        var customPreRepeat = "G92 E5 ; custom reset";

        // Act
        settingsInfo.PreRepeatingGcode = customPreRepeat;

        // Assert
        Assert.AreEqual(customPreRepeat, settingsInfo.PreRepeatingGcode);
    }

    [TestMethod]
    public void GcodeRepeatingSettingsInfo_CustomRepeatingGcode_SetsValue()
    {
        // Arrange
        var settingsInfo = new GcodeRepeatingSettingsInfo();
        var customRepeat = "G1 Y200 F5000 ; custom move";

        // Act
        settingsInfo.RepeateGcode = customRepeat;

        // Assert
        Assert.AreEqual(customRepeat, settingsInfo.RepeateGcode);
    }
}