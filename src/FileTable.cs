using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTables {

  public class FileTable {

    public FileTable() { }
    public FileTable(string fileName) {      
      FileName = fileName;
      Active = true;
    }
    private bool _Active = false;
    public bool Active {
      get {
        return _Active;
      }
      set {
        if ((!_Active) && (value)) {          
          if (File.Exists(FileName)) {
            LoadFromFile();            
          } else {            
            this.Package = new FilePackage();
          }
          _Active = true;
        } else if ((_Active) && (!value)) {
          _Active = false;
          this.Package = new FilePackage();          
        }
      }
    }

    public string FileName { get; set; } = "";
    public FilePackage Package { get; set; } = new FilePackage();

    public Columns Columns { 
      get { return new Columns(Package.Columns); }
      set { Package.Columns = value.AsList; }
    }

    public Fields Fields {
      get { return new Fields(Package.Fields, Columns); }
      set { Package.Fields = value.AsList; }
    }

    public Rows Rows {
      get { return new Rows(this, Package.Rows); }
      set { Package.Rows = value.AsList; }
    }    

    public void LoadFromFile() {
      if (File.Exists(FileName)) {
        var encoded = Task.Run(async () => await FileName.ReadAllTextAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
        if (encoded.Length ==0) return;
        var decoded = Convert.FromBase64String(encoded);
        this.Package = MessagePack.MessagePackSerializer.Deserialize<FilePackage>(decoded);
      }
    }

    public void SaveToFile() {
      var rawBytes = MessagePack.MessagePackSerializer.Serialize(this.Package);
      var encoded = Convert.ToBase64String(rawBytes);
      Task.Run(async () => await encoded.WriteAllTextAsync(FileName).ConfigureAwait(false)).GetAwaiter().GetResult();
    }

    public IEnumerable<FieldModel> GetFieldsOfRow(int rowId) {
      return this.Package.Fields.Where(x => x.RowId == rowId);
    }

    public int GetColumnID(string columnName) {
      return this.Package.Columns.FirstOrDefault(x => x.ColumnName == columnName)?.Id ?? 0;
    }

    public ColumnType GetColumnType(string columnName) {
      return (ColumnType)(this.Package.Columns.FirstOrDefault(x => x.ColumnName == columnName)?.ColumnType ?? 0);
    }

    public void EnsureColumn(string columnName, ColumnType columnType) {
      if (!this.Package.Columns.Any(x => x.ColumnName == columnName)) {
        AddColumn(columnName, columnType);
      }
    }

    public ColumnModel AddColumn(string columnName, ColumnType columnType) {
      Columns tblCols = new(this.Package.Columns);
      Rows tblRows = new(this, this.Package.Rows);
      Fields tblFields = new(this.Package.Fields, tblCols);

      var col = tblCols.Add(new ColumnModel() { 
        ColumnName = columnName, 
        ColumnType = (short)columnType       
      });   
      col.Rank = col.Id;

      foreach (var row in tblRows.Values) {
        var aFld = tblFields.Add(new FieldModel() {
          ColumnId = col.Id,
          RowId = row.Id,
          ValueString = ""
        });
        row.RowFields.Add(aFld);
      }

      this.Package.Fields = tblFields.AsList;
      this.Package.Rows = tblRows.AsList;
      this.Package.Columns = tblCols.AsList;
      return col;
    }

    public void RemoveColumn(int columnId) {
      Columns tblCols = new(this.Package.Columns);
      Rows tblRows = new(this, this.Package.Rows);
      Fields tblFields = new(this.Package.Fields, tblCols);
      foreach (var fld in tblFields.Where(x => x.Value.ColumnId == columnId)) {
        tblFields.Remove(fld.Value);
      }
      tblCols.Remove(columnId);
      this.Package.Fields = tblFields.AsList;
      this.Package.Rows = tblRows.AsList;
      this.Package.Columns = tblCols.AsList;
    }

    public RowModel AddRow() {
      Columns tblCols = new(this.Package.Columns);
      Rows tblRows = new(this, this.Package.Rows);
      Fields tblFields = new(this.Package.Fields, tblCols);

      var row = tblRows.Add(new RowModel(this));
      foreach (var col in this.Package.Columns.OrderBy(x => x.Rank)) {       
        var aFld = tblFields.Add(new FieldModel() {
          ColumnId = col.Id,
          RowId = row.Id,
          ValueType = (ColumnType)col.ColumnType,
          ValueString = ""
        });
        row.RowFields.Add(aFld);
      }

      this.Package.Fields = tblFields.AsList;
      this.Package.Rows = tblRows.AsList;
      return row;
    }

    public void RemoveRow(int rowId) {
      Columns tblCols = new(this.Package.Columns);
      Rows tblRows = new(this, this.Package.Rows);
      Fields tblFields = new(this.Package.Fields, tblCols);
      
      foreach (var fld in tblFields.Where(x => x.Value.RowId == rowId)) {
        tblFields.Remove(fld.Value);
      }
      tblRows.Remove(rowId);
      
      this.Package.Fields = tblFields.AsList;
      this.Package.Rows = tblRows.AsList;
    }
  }

}
