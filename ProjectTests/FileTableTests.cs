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

      _fileTable.SetActive(true);
      Assert.IsTrue(_fileTable.Active);

      _fileTable.SetActive(false);
      Assert.IsFalse(_fileTable.Active);
    }

    [TestMethod]
    public void TestAddColumn() {
      var column = _fileTable.AddColumn("TestColumn", ColumnType.String);
      Assert.IsNotNull(column);
      Assert.AreEqual("TestColumn", column.Name);
      Assert.AreEqual(ColumnType.String, column.Type);
      Assert.IsTrue(_fileTable.Columns.Contains(column.Id));
    }

    [TestMethod]
    public void TestAddRow() {
      long rowId = _fileTable.AddRow();
      Assert.IsTrue(_fileTable.Rows.Contains(rowId));
    }

    [TestMethod]
    public void TestLoad() {
      // Simulate file content
      var package = new TableWirePackage();
      string content = Convert.ToBase64String(MessagePackSerializer.Serialize(package));
      File.WriteAllText(_tempFileName, content);

      _fileTable.SetActive(true);
      Assert.IsTrue(_fileTable.Active);
    }

    [TestMethod]
    public void TestSave() {
      _fileTable.AddColumn("TestColumn", ColumnType.String);
      _fileTable.AddRow();
      _fileTable.Save();

      Assert.IsTrue(File.Exists(_tempFileName));
      string content = File.ReadAllText(_tempFileName);
      Assert.IsFalse(string.IsNullOrEmpty(content));
    }

    [TestMethod]
    public async Task TestLoadAsync() {
      // Simulate file content
      var package = new TableWirePackage();
      string content = Convert.ToBase64String(MessagePackSerializer.Serialize(package));
      await File.WriteAllTextAsync(_tempFileName, content);

      await _fileTable.LoadAsync();
      Assert.IsTrue(_fileTable.Active);
    }

    [TestMethod]
    public async Task TestSaveAsync() {
      _fileTable.AddColumn("TestColumn", ColumnType.String);
      _fileTable.AddRow();
      await _fileTable.SaveAsync();

      Assert.IsTrue(File.Exists(_tempFileName));
      string content = await File.ReadAllTextAsync(_tempFileName);
      Assert.IsFalse(string.IsNullOrEmpty(content));
    }
  }
}

