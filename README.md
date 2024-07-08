# FileTable 
## Library of routines implementing a tables stored in text files.
### [install from NuGet link](https://www.nuget.org/packages/FileTables)
### Latest is 1.0.3, adds try blocks around load and save, empty value handeling.
### Version 1.0.2 updates row to long.
* ### By table I mean three properties.
  * ### FileName - to save the location of the file.
  * ### Columns - concurrent dictionary of columns
  * ### Rows - Concurrent dictionary of rows where
    * #### Each Row is a Dictionary of Fields that match the Columns.
* ### By stored in text files with MessagePack means
  * #### Creates a wirepack object that has 2 properties a list of column and list of row.
    * #### On Save the code copies the Dictionary to List in wirepack
      * #### uses MessagPack to translate the wirepack into Byte[] in which I convert to string via Base64 encoding the buffer.
      * #### uses a stream to write the encoding to file.
    * #### On Load it's reverse, Load the encoded string from file.
      * #### Decode Base 64 to byte[] then use MessagePack to deserialize back to wirepack object.
        * #### from wirepack object copy the list of columns and rows to active Model.   
   
## Each FileTable can be created from scratch. 
  * Setting Active = true; loads from file if one exists. 
  * if your table didn't exist then the developer needs to add the columns before adding rows.
  * below is an example of how one could wrap the FileTable.
  * below is sample output from [AppSmith App](https://github.com/mmeents/AppSmith) 

```csharp
 public class Settings {  // the entity class that represents the row we are modeling
    public Settings() { }
    public long Id { get; set; } = 0;
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
  }

  public class SettingsFileTable {  //  FileTable Wrapper that supports basic REST ops... from UnitTests
    private readonly FileTable _table;
    public SettingsFileTable(string fileName) {
      _table = new FileTable(fileName);
      _table.Active = true;
      if (_table.Columns.Count() == 0) {
        _table.AddColumn("Id", ColumnType.Int64);
        _table.AddColumn("Name", ColumnType.String);
        _table.AddColumn("Value", ColumnType.String);
      }
    }
    public Columns Columns { get { return _table.Columns; }}
    public Rows Rows { get { return _table.Rows; }}

    public Settings? Get(long id) { 
      if (_table.Rows.Contains(id)) {
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
      long RowKey = item.Id;
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
```