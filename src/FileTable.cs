using MessagePack;
using System.Collections.Concurrent;
using System.Text;

namespace FileTables {
  
  public class FileTable {

    private bool _Active = false;
    public bool Active { get { return GetActive(); } set { SetActive(value); } }
    public string FileName { get; set; }
    public TableWirePackage Package { get; set; }
    public Columns Columns { get; set; }
    public Rows Rows { get; set; }

    public FileTable(string fileName) {
      FileName = fileName;
      Columns = new Columns();
      Rows = new Rows(this, Columns);
      Package = new TableWirePackage();

    }

    public bool GetActive() {
      return _Active;
    }
    public void SetActive(bool value) {
      if ((!_Active) && (value)) {
        if (File.Exists(FileName)) {
          Load();
          _Active = true;          
        }
      } else if ((_Active) && (!value)) {
        _Active = false;        
        Columns.Clear();
        Rows.Clear();
      }
    }
    public void Load() {
      Task.Run(async () => await this.LoadAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
    }
    public async Task LoadAsync() {
      if (File.Exists(FileName)) {
        var encoded = await FileName.ReadAllTextAsync();
        if (encoded.Length > 0) { 
          var decoded = Convert.FromBase64String(encoded);
          this.Package = MessagePackSerializer.Deserialize<TableWirePackage>(decoded);
          Columns.AsList = this.Package.Columns;
          Rows.AsList = this.Package.Rows;
        }
        if (!_Active) {
          _Active = true;
        }
      }
    }
    public void Save() {
      Task.Run(async () => await this.SaveAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
    }
    public async Task SaveAsync() {
      this.Package.Columns = Columns.AsList;
      this.Package.Rows = Rows.AsList;
      byte[] WirePacked = MessagePackSerializer.Serialize(this.Package);
      string encoded = Convert.ToBase64String(WirePacked);
      await encoded.WriteAllTextAsync(FileName);
    }

    public Column AddColumn(string ColumnName, ColumnType ColumnType) {
      var col = new Column(this.Columns) {
        Name = ColumnName, Type = ColumnType, Id = 0
      };
      this.Columns[col.Id] = col;
      return col;
    }

    public long AddRow() {
      Row ret = new Row(Rows);
      this.Rows[ret.Key] = ret;
      return ret.Key;
    }

  }

  public static class FileTableExt {
    public static async Task<string> ReadAllTextAsync(this string filePath) {
      using (var streamReader = new StreamReader(filePath)) {
        return await streamReader.ReadToEndAsync();
      }
    }

    public static async Task<int> WriteAllTextAsync(this string Content, string filePath) {
      using (var streamWriter = new StreamWriter(filePath)) {
        await streamWriter.WriteAsync(Content);
      }
      return 1;
    }
  }




}