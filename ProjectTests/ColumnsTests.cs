using FileTables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ProjectTests {
  [TestClass]
  public class ColumnsTests {
    private Columns _columns;

    [TestInitialize]
    public void Setup() {
      _columns = new Columns();
    }

    [TestMethod]
    public void TestColumnsInitialization() {
      Assert.AreEqual(0, _columns.Count);
    }

    [TestMethod]
    public void TestAddColumn() {
      var column = new Column(_columns) { Name = "TestColumn", Type = ColumnType.String };
      _columns[1] = column;
      Assert.AreEqual(1, _columns.Count);
      Assert.IsTrue(_columns.Contains(1));
      Assert.IsTrue(_columns.Contains("TestColumn"));
    }

    [TestMethod]
    public void TestRemoveColumn() {
      var column = new Column(_columns) { Name = "TestColumn", Type = ColumnType.String };
      _columns[1] = column;
      _columns.Remove(1);
      Assert.AreEqual(0, _columns.Count);
      Assert.IsFalse(_columns.Contains(1));
      Assert.IsFalse(_columns.Contains("TestColumn"));
    }

    [TestMethod]
    public void TestGetNextId() {
      var column1 = new Column(_columns) { Name = "Column1", Type = ColumnType.String };
      _columns[1] = column1;
      var column2 = new Column(_columns) { Name = "Column2", Type = ColumnType.Int32 };
      _columns[2] = column2;
      Assert.AreEqual(3, _columns.GetNextId());
    }

    [TestMethod]
    public void TestAsList() {
      var column1 = new Column(_columns) { Name = "Column1", Type = ColumnType.String };
      _columns[1] = column1;
      var column2 = new Column(_columns) { Name = "Column2", Type = ColumnType.Int32 };
      _columns[2] = column2;

      var list = _columns.AsList;
      Assert.AreEqual(2, list.Count);
    }

    [TestMethod]
    public void TestAsEncoded() {
      var column = new Column(_columns) { Name = "TestColumn", Type = ColumnType.String };
      _columns[1] = column;
      string encoded = column.AsEncoded();
      Assert.IsFalse(string.IsNullOrEmpty(encoded));
    }

    [TestMethod]
    public void TestAsDecoded() {
      var column = new Column(_columns) { Name = "TestColumn", Type = ColumnType.String };
      _columns[1] = column;
      string encoded = column.AsEncoded();
      var decodedColumn = new Column(_columns).AsDecoded(encoded);
      Assert.AreEqual(column.Name, decodedColumn.Name);
      Assert.AreEqual(column.Type, decodedColumn.Type);
      Assert.AreEqual(column.Id, decodedColumn.Id);
    }
  }
}
