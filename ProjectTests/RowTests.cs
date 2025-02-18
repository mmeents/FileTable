using FileTables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectTests {
  [TestClass]
  public class RowTests {
    private FileTable _fileTable;
    private Columns _columns;
    private Rows _rows;
    private long _rowId;
    private Row _row;

    [TestInitialize]
    public void Setup() {
      _fileTable = new FileTable("TestTable");
      _columns = _fileTable.Columns;
      _fileTable.AddColumn("Column1", ColumnType.String);
      _fileTable.AddColumn("Column2", ColumnType.Int32);      
      _rows = _fileTable.Rows;
      _rowId = _fileTable.AddRow();
      _row = _rows[_rowId];
    }

    [TestMethod]
    public void TestRowInitialization() {
      Assert.AreEqual(_rows, _row.Owner);
      Assert.AreEqual(2, _row.Count);
      Assert.IsTrue(_row.Contains("Column1"));
      Assert.IsTrue(_row.Contains("Column2"));
    }

    [TestMethod]
    public void TestRowAddField() {
      var field = new Field(_row, _columns.ByName("Column1"), "TestValue");
      _row.Add(field);
      Assert.AreEqual(field, _row["Column1"]);
    }

    [TestMethod]
    public void TestRowIndexer() {
      var field = new Field(_row, _columns.ByName("Column1"), "TestValue");
      _row["Column1"] = field;
      Assert.AreEqual(field, _row["Column1"]);

      _row["Column1"] = null;
      Assert.IsNull(_row["Column1"]);
    }

    [TestMethod]
    public void TestRowAsEncoded() {
      var field1 = new Field(_row, _columns.ByName("Column1"), "TestValue");
      var field2 = new Field(_row, _columns.ByName("Column2"), "123");
      _row.Add(field1);
      _row.Add(field2);

      string encoded = _row.AsEncoded();
      Assert.IsFalse(string.IsNullOrEmpty(encoded));
    }

    [TestMethod]
    public void TestRowAsDecoded() {
      _row["Column1"].Value = "TestValue";
      _row["Column2"].Value = "123";  

      string encoded = _row.AsEncoded();
      Row? decodedRow = _rows.AsDecoded(encoded);
      Assert.IsNotNull(decodedRow);
      Assert.AreEqual(_row.Key, decodedRow.Key);
      Assert.AreEqual(_row["Column1"].Value, decodedRow["Column1"].Value);
      Assert.AreEqual(_row["Column2"].Value, decodedRow["Column2"].Value);
    }
  }
}
