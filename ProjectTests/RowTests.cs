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
    private int _rowId;
    private RowModel _row;

    [TestInitialize]
    public void Setup() {
      _fileTable = new FileTable("TestTable");
      _columns = _fileTable.Columns;
      _fileTable.AddColumn("Column1", ColumnType.String);
      _fileTable.AddColumn("Column2", ColumnType.Int32);      
      _rows = _fileTable.Rows;
      _row = _fileTable.AddRow();
      _row = _rows[_row.Id];
    }

    [TestMethod]
    public void TestRowInitialization() {
      Assert.AreEqual(_rows, _row.Owner);
      Assert.AreEqual(2, _row.RowFields.Count());      
    }


  }
}
