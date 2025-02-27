using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTables {

  [MessagePackObject]
  public class RowModel {    
    public RowModel() { } 
    public RowModel(FileTable owner) {
      Owner = owner;
    }

    [Key(0)]
    public int Id { get; set; } =0;

    [IgnoreMember]
    public FileTable? Owner { get; set; } = null;

    [IgnoreMember]
    public Fields RowFields{ get; set; }

    [IgnoreMember]
    public FieldModel? this[string columnName] {
      get {
        if (Owner == null || columnName ==null || columnName.Length ==0) return null;
        var columnId = Owner!.GetColumnID(columnName);
        FieldModel? field = RowFields.Values.FirstOrDefault(x => x.ColumnId == columnId);       
        return field;
      }
      set {
        if (Owner == null || columnName == null || columnName.Length == 0) return;
        var columnId = Owner!.GetColumnID(columnName);
        if (value != null) {         
          value.ColumnId = columnId;
          RowFields.Add(value);
        } else {          
          var field = RowFields.Values.FirstOrDefault(x => x.ColumnId == columnId);
          if (field != null) {
            RowFields.Remove(field);
          }
        }
      }
    }

  }

}
