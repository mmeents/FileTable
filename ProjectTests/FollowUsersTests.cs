using FileTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTests {
  [TestClass]
public class testFollowUsers {
    
}



public class FollowedUser {
  public FollowedUser() { }
  public long Uid { get; set; } = 0;  // this is for the row ID.
  public int FollowCount { get; set; } = 0;
  public int FollowStatus { get; set; } = 0;
  public int Id { get; set; } = 0;
  public string Login { get; set; } = "";
}


  public class FollowedUserFileTable {
    private readonly FileTable _table;
    public Columns Columns { get { return _table.Columns; } }
    public Rows Rows { get { return _table.Rows; } }
    public FollowedUserFileTable(string fileName) {
      _table = new FileTable(fileName);
      _table.Active = true;
      if (_table.Columns.Count() == 0) {
        _table.AddColumn("Uid", ColumnType.Int64);
        _table.AddColumn("FollowCount", ColumnType.Int32);
        _table.AddColumn("FollowStatus", ColumnType.Int32);
        _table.AddColumn("Id", ColumnType.Int64);
        _table.AddColumn("Login", ColumnType.String);
      }
    }
   



    public void Insert(FollowedUser item) {
      var RowKey = _table.AddRow();
      _table.Rows[RowKey.Id]["Uid"].Value = RowKey.Id;
      _table.Rows[RowKey.Id]["FollowCount"].Value = item.FollowCount;
      _table.Rows[RowKey.Id]["FollowStatus"].Value = item.FollowStatus;
      _table.Rows[RowKey.Id]["Id"].Value = item.Id;
      _table.Rows[RowKey.Id]["Login"].Value = item.Login;
      //_table.Save();
    }
    public void Update(FollowedUser item) {
      var RowKey = item.Id;
      _table.Rows[RowKey]["Uid"].Value = item.Uid;
      _table.Rows[RowKey]["FollowCount"].Value = item.FollowCount;
      _table.Rows[RowKey]["FollowStatus"].Value = item.FollowStatus;
      _table.Rows[RowKey]["Id"].Value = item.Id;
      _table.Rows[RowKey]["Login"].Value = item.Login;
      _table.SaveToFile();
    }
  }
}


