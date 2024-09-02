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
        long RowId = testTbl.AddRow();
        testTbl.Rows[RowId]["Id"].Value = i.ToString();
        testTbl.Rows[RowId]["Name"].Value = $"Name {i}";
        testTbl.Rows[RowId]["Value"].Value = $"Value {i}";
      }
      Console.WriteLine($"{DateTime.Now} Filled before save");
      testTbl.Save();
      Console.WriteLine($"{DateTime.Now} After Filled Save");
      foreach (Row r in  testTbl.Rows.Values) {
        Console.WriteLine(r["Name"].Value+" " + r["Value"].Value); 
      }
      Console.WriteLine($"{DateTime.Now} before load");
      var testTbl2 = new FileTable(fileName);
      testTbl2.Active = true;
      Console.WriteLine($"{DateTime.Now} after load");
      foreach (Row r in testTbl2.Rows.Values) {
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
      
      for (var i = 1; i < 1000; i++) {
        var RowSetting = new Settings() {  
        Id = i,
        Name = $"Name {i}",
        Value = $"Value {i}"
        };
        setTbl.Insert(RowSetting);
      }

      var RowSetting1 = new Settings() {
        Id = 50,
        Name = $"Name THIS is 50",
        Value = $"Name THIS is 50"
      };
      setTbl.Update(RowSetting1);

      var RowSetting2 = new Settings() {
        Id = 100,
        Name = $"Name THIS is 100",
        Value = $"Name THIS is 100"
      };
      setTbl.Delete(RowSetting2);

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

    public Settings? Get(int id) { 
      if (_table.Rows.ContainsKey(id)) {
        return new Settings(){
          Id = id,
          Name = _table.Rows[id]["Name"].Value,
          Value = _table.Rows[id]["Value"].Value
        };
      } else { 
        return null;
      }
    }
    public void Insert(Settings item) {
      long RowKey = _table.AddRow();
      _table.Rows[RowKey]["Id"].Value = item.Id.AsString();
      _table.Rows[RowKey]["Name"].Value = item.Name;
      _table.Rows[RowKey]["Value"].Value = item.Value;
      _table.Save();
    }
    public void Update(Settings item) {
      int RowKey = item.Id;
      _table.Rows[RowKey]["Id"].Value = item.Id.AsString();
      _table.Rows[RowKey]["Name"].Value = item.Name;
      _table.Rows[RowKey]["Value"].Value = item.Value;
      _table.Save();
    }
    public void Delete(Settings item) {
      int RowKey = item.Id;
      _table.Rows.Remove(RowKey, out Row? _);
      _table.Save();
    }
  }

}