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
      var column = new ColumnModel(){ Rank=1, ColumnName = "TestColumn", ColumnType = (short)ColumnType.String };
      _columns[1] = column;
      Assert.AreEqual(1, _columns.Count);
      Assert.IsTrue(_columns.Contains(1));
      Assert.IsTrue(_columns.Contains("TestColumn"));
    }

    [TestMethod]
    public void TestRemoveColumn() {
      var column = new ColumnModel() { Rank = 1, ColumnName = "TestColumn", ColumnType = (short)ColumnType.String };
      _columns[1] = column;
      _columns.Remove(1);
      Assert.AreEqual(0, _columns.Count);
      Assert.IsFalse(_columns.Contains(1));
      Assert.IsFalse(_columns.Contains("TestColumn"));
    }

    [TestMethod]
    public void TestGetNextId() {
      var column1 = new ColumnModel() { Rank = 1, ColumnName = "Column1", ColumnType = (short)ColumnType.String };
      _columns[1] = column1;
      var column2 = new ColumnModel() { Rank = 1, ColumnName = "Column2", ColumnType = (short)ColumnType.Int32 };
      _columns[2] = column2;
      Assert.AreEqual(3, _columns.GetNextId());
    }

    [TestMethod]
    public void TestAsList() {
      var column1 = new ColumnModel() { Rank = 1, ColumnName = "Column1", ColumnType = (short)ColumnType.String };
      _columns[1] = column1;
      var column2 = new ColumnModel() { Rank = 1, ColumnName = "Column2", ColumnType = (short)ColumnType.Int32 };
      _columns[2] = column2;

      var list = _columns.AsList;
      Assert.AreEqual(2, list.Count());
    }
    
  }
}
