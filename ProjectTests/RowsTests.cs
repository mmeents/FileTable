using FileTables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ProjectTests {
  [TestClass]
  public class RowsTests {
    private FileTable _fileTable;
    private Columns _columns;
    private Rows _rows;

    [TestInitialize]
    public void Setup() {
      _fileTable = new FileTable("TestTable");
      _columns = _fileTable.Columns;
      _columns[1] = new Column(_columns) { Name = "Column1", Type = ColumnType.String };
      _columns[2] = new Column(_columns) { Name = "Column2", Type = ColumnType.Int32 };
      _rows = _fileTable.Rows;
    }

    [TestMethod]
    public void TestRowsInitialization() {
      Assert.AreEqual(_fileTable, _rows.Owner);
      Assert.AreEqual(_columns, _rows.Cols);
      Assert.AreEqual(0, _rows.Count);
    }

    [TestMethod]
    public void TestAddRow() {
      var row = new Row(_rows);
      _rows.Add(row);
      Assert.AreEqual(1, _rows.Count);
      Assert.IsTrue(_rows.Contains(row.Key));
    }

    [TestMethod]
    public void TestRemoveRow() {
      var row = new Row(_rows);
      _rows.Add(row);
      _rows.Remove(row.Key);
      Assert.AreEqual(0, _rows.Count);
      Assert.IsFalse(_rows.Contains(row.Key));
    }

    [TestMethod]
    public void TestGetNextId() {
      var row1 = new Row(_rows);
      _rows.Add(row1);
      var row2 = new Row(_rows);
      _rows.Add(row2);
      Assert.AreEqual(3, _rows.GetNextId());
    }

    [TestMethod]
    public void TestAsList() {
      var row1 = new Row(_rows);
      _rows.Add(row1);
      var row2 = new Row(_rows);
      _rows.Add(row2);

      var list = _rows.AsList;
      Assert.AreEqual(2, list.Count);
    }

    [TestMethod]
    public void TestAsEncoded() {
      var row = new Row(_rows);
      _rows.Add(row);
      string encoded = row.AsEncoded();
      Assert.IsFalse(string.IsNullOrEmpty(encoded));
    }

    [TestMethod]
    public void TestAsDecoded() {
      var row = new Row(_rows);
      _rows.Add(row);
      string encoded = row.AsEncoded();
      Row? decodedRow = _rows.AsDecoded(encoded);
      Assert.IsNotNull(decodedRow);
      Assert.AreEqual(row.Key, decodedRow.Key);
    }
  }
}
