using FileTables;
using FileTables.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProjectTests {

  [TestClass]
  public class SettingsFileTests {
    private const string TestFileName = "testSettingsFile.dat";
    private MockLogMsg _mockLogMsg;

    [TestInitialize]
    public void Setup() {
      _mockLogMsg = new MockLogMsg();
      if (File.Exists(TestFileName)) {
        File.Delete(TestFileName);
      }
    }

    [TestCleanup]
    public void Cleanup() {
      if (File.Exists(TestFileName)) {
        File.Delete(TestFileName);
      }
    }

    [TestMethod]
    public void TestLoadAndSaveSettings() {
      // Arrange
      var settingsFile = new SettingsFile(TestFileName, _mockLogMsg);
      var settings = new FileTables.Settings();
      settings["Setting1"].Value = "Value1";
      settings["Setting2"] = new SettingProperty { Key = "Setting2", Value = "Value2" };
    
      settingsFile.Settings = settings;

      // Act
      settingsFile.Save();
      settingsFile.Load();

      // Assert
      Assert.IsTrue(settingsFile.FileLoaded);
      Assert.AreEqual(2, settingsFile.Settings.Count);
      Assert.AreEqual("Value1", settingsFile.Settings["Setting1"].Value);
      Assert.AreEqual("Value2", settingsFile.Settings["Setting2"].Value);
    }

    [TestMethod]
    public async Task TestLoadAndSaveSettingsAsync() {
      // Arrange
      var settingsFile = new SettingsFile(TestFileName, _mockLogMsg);
      var settings = new FileTables.Settings();
      settings["Setting1"] = new SettingProperty { Key = "Setting1", Value = "Value1" };
      settings["Setting2"] = new SettingProperty { Key = "Setting2", Value = "Value2" };
      settingsFile.Settings = settings;

      // Act
      await settingsFile.SaveAsync();
      await settingsFile.LoadAsync();

      // Assert
      Assert.IsTrue(settingsFile.FileLoaded);
      Assert.AreEqual(2, settingsFile.Settings.Count);
      Assert.AreEqual("Value1", settingsFile.Settings["Setting1"].Value);
      Assert.AreEqual("Value2", settingsFile.Settings["Setting2"].Value);
    }

    [TestMethod]
    public void TestCloneSettings() {
      // Arrange
      var settings = new Dictionary<string, SettingProperty> {
        { "Setting1", new SettingProperty { Key = "Setting1", Value = "Value1" } },
        { "Setting2", new SettingProperty { Key = "Setting2", Value = "Value2" } }
      };

      // Act
      var clonedSettings = new Dictionary<string, SettingProperty>(settings);

      // Assert
      Assert.AreEqual(2, clonedSettings.Count);
      Assert.AreEqual("Value1", clonedSettings["Setting1"].Value);
      Assert.AreEqual("Value2", clonedSettings["Setting2"].Value);
    }
  }

  public class MockLogMsg : ILogMsg {
    public void LogMsg(string msg) {
      Console.WriteLine(msg);
    }
  }
}
