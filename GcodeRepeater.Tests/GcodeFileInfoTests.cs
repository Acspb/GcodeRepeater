using Microsoft.VisualStudio.TestTools.UnitTesting;
using GcodeRepeater.Gcode;

namespace GcodeRepeater.Tests;

[TestClass]
public class GcodeFileInfoTests
{
    [TestMethod]
    public void GcodeFileInfo_DefaultValues_AreNull()
    {
        // Arrange & Act
        var fileInfo = new GcodeFileInfo();

        // Assert
        Assert.IsNull(fileInfo.LayerCount);
        Assert.IsNull(fileInfo.StartOfGcodeBodyIndex);
        Assert.IsNull(fileInfo.EndOfGcodeBodyIndex);
        Assert.IsNull(fileInfo.EndOfGcode);
        Assert.IsNull(fileInfo.RepeateIndex);
    }

    [TestMethod]
    public void GcodeFileInfo_SetProperties_ValuesAreCorrect()
    {
        // Arrange
        var fileInfo = new GcodeFileInfo();
        
        // Act
        fileInfo.LayerCount = 10;
        fileInfo.StartOfGcodeBodyIndex = 5;
        fileInfo.EndOfGcodeBodyIndex = 100;
        fileInfo.EndOfGcode = 200;
        fileInfo.RepeateIndex = 150;

        // Assert
        Assert.AreEqual(10, fileInfo.LayerCount);
        Assert.AreEqual(5, fileInfo.StartOfGcodeBodyIndex);
        Assert.AreEqual(100, fileInfo.EndOfGcodeBodyIndex);
        Assert.AreEqual(200, fileInfo.EndOfGcode);
        Assert.AreEqual(150, fileInfo.RepeateIndex);
    }
}