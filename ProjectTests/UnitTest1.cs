using System.IO;
using FileTables;
using static ProjectTests.UnitTest1;

namespace ProjectTests {
  [TestClass]
  public class UnitTest1 {

    [TestMethod]
    public void TestMethod1() {
      Console.WriteLine($"{DateTime.Now} Starting");

      var fileName = Path.GetTempFileName();

      var testTbl = new FileTable(fileName);
      testTbl.Active = true;
      if (testTbl.Columns.Count == 0) {
        testTbl.AddColumn("Id", ColumnType.Int32);
        testTbl.AddColumn("Name", ColumnType.String);
        testTbl.AddColumn("Value", ColumnType.String);
      }

      for (var i = 1; i<1000; i++) { 
        var row = testTbl.AddRow();
        testTbl.Rows[row.Id]["Id"].Value = i.ToString();
        testTbl.Rows[row.Id]["Name"].Value = $"Name {i}";
        testTbl.Rows[row.Id]["Value"].Value = $"Value {i}";
      }
      Console.WriteLine($"{DateTime.Now} Filled before save");
      testTbl.SaveToFile();
      Console.WriteLine($"{DateTime.Now} After Filled Save");
      foreach (RowModel r in  testTbl.Rows.Values) {
        Console.WriteLine(r["Name"].Value+" " + r["Value"].Value); 
      }
      Console.WriteLine($"{DateTime.Now} before load");
      var testTbl2 = new FileTable(fileName);
      testTbl2.Active = true;
      Console.WriteLine($"{DateTime.Now} after load");
      foreach (RowModel r in testTbl2.Rows.Values) {
        Console.WriteLine(r["Name"].Value + " " + r["Value"].Value);
      }
      Console.WriteLine($"{DateTime.Now} after output");

      File.Delete(fileName);
      Console.WriteLine($"{DateTime.Now} after delete");
    }

    [TestMethod]
    public void TestMethod2() {

      var fileName = Path.GetTempFileName();
      var setTbl = new SettingsFileTable(fileName);
      

      foreach(var key in setTbl.Rows.Keys) { 
        Console.WriteLine($"{key} -> Name:{setTbl.Rows[key]["Name"].Value}; Value:{setTbl.Rows[key]["Value"].Value}");
      }


    }
  }

  public class Settings {
    public Settings() { }
    public int Id { get; set; } = 0;
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
  }

  public class SettingsFileTable {
    private readonly FileTable _table;
    public SettingsFileTable(string fileName) {
      _table = new FileTable(fileName);
      _table.Active = true;
      if (_table.Columns.Count() == 0) {
        _table.AddColumn("Id", ColumnType.Int32);
        _table.AddColumn("Name", ColumnType.String);
        _table.AddColumn("Value", ColumnType.String);
      }
    }
    public Columns Columns { get { return _table.Columns; }}
    public Rows Rows { get { return _table.Rows; }}


  }

}