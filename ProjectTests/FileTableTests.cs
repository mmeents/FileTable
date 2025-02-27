using FileTables;
using MessagePack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProjectTests {
  [TestClass]
  public class FileTableTests {
    private FileTable _fileTable;
    private string _tempFileName;

    [TestInitialize]
    public void Setup() {
      _tempFileName = Path.GetTempFileName();
      _fileTable = new FileTable(_tempFileName);
    }

    [TestCleanup]
    public void Cleanup() {
      if (File.Exists(_tempFileName)) {
        File.Delete(_tempFileName);
      }
    }

    [TestMethod]
    public void TestFileTableInitialization() {
      Assert.AreEqual(_tempFileName, _fileTable.FileName);
      Assert.IsNotNull(_fileTable.Columns);
      Assert.IsNotNull(_fileTable.Rows);
      Assert.IsNotNull(_fileTable.Package);
      Assert.IsFalse(_fileTable.Active);
    }

    [TestMethod]
    public void TestSetActive() {
      // Ensure the file exists
      File.WriteAllText(_tempFileName, string.Empty);

      _fileTable.Active =true;
      Assert.IsTrue(_fileTable.Active);

      _fileTable.Active = false;
      Assert.IsFalse(_fileTable.Active);
    }

    

    
  }
}

