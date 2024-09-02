using MessagePack;
using System.Collections.Concurrent;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace FileTables {

  public static class FldExt {
    public static string AsEncoded(this Field field) {
      if (field == null) { return ""; }
      string value = field.Value;
      value = value == ""? "<null>" : value;
      return $"{field.Column.Name.AsBase64Encoded()} {value.AsBase64Encoded()}".AsBase64Encoded();
    }
    public static Field? AsDecoded(this Row row, string encoded) {
      if (!string.IsNullOrEmpty(encoded)) {
        var vals = encoded.AsBase64Decoded().Parse(" ");
        var colName = vals[0].AsBase64Decoded();
        var col = row.Owner.Cols.ByName(colName);
        var value = vals[1].AsBase64Decoded();
        value = value == "<null>"?"":value;        
        if (col != null) {
          return new Field(row, col, value);
        }
      }
      return null;
    }

    public static string AsEncoded(this Row row) {
      if (row == null) { return ""; }
      StringBuilder sb = new StringBuilder();
      sb.Append($"{row.Key} ");
      IOrderedEnumerable<int> columnKeys = row.Owner.Cols.Keys.OrderBy(x => x);
      foreach (var columnKey in columnKeys) {
        var col = row.Owner.Cols[columnKey];
        if (col != null) {
          var field = row[col.Name];
          if (field != null) {
            sb.Append(field.AsEncoded() + " ");
          }
        }
      }
      return sb.ToString().AsBase64Encoded();
    }
    public static Row? AsDecoded(this Rows rows, string encoded) {
      if (rows == null) { throw new ArgumentNullException(nameof(rows)); }
      if (!string.IsNullOrEmpty(encoded)) {
        var vals = encoded.AsBase64Decoded().Parse(" ");
        var len = vals.Length;
        if (len > 0) {
          var ret = new Row(rows) { Key = vals[0].AsInt64() };
          if (len > 1) {
            for (int i = 1; i < len; i++) {
              try { 
                Field? newField = ret.AsDecoded(vals[i]);
                if (newField != null) {
                  ret.Add(newField);
                }
              } catch { }
            }
          }
          rows[ret.Key] = ret;
          return ret;
        }
      }
      return null;
    }

    public static int AsInt32(this string value){ 
      return int.Parse(value);
    }
    public static long AsInt64(this string value) { 
      return long.Parse(value); 
    }
    public static byte[] AsByte(this string value) {
      return Convert.FromBase64String(value);
    }
    public static bool AsBoolean(this string value) { 
      return Convert.ToBoolean(value);
    }
    public static DateTime AsDateTime(this string value) { 
      return DateTime.Parse(value);
    }
    public static Decimal AsDecimal(this string value) { 
      return Decimal.Parse(value);
    }    
    public static byte[] AsBytes(this string text) {
      return Encoding.UTF8.GetBytes(text);
    }    
    public static string AsString(this byte[] bytes) {
      return Encoding.UTF8.GetString(bytes);
    }
    
    public static string[] Parse(this string content, string delims) {
      return content.Split(delims.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    }    
    public static string ParseFirst(this string content, string delims) {
      string[] sr = content.Parse(delims);
      if (sr.Length > 0) {
        return sr[0];
      }
      return "";
    }    
    public static string ParseLast(this string content, string delims) {
      string[] sr = content.Parse(delims);
      if (sr.Length > 0) {
        return sr[sr.Length - 1];
      }
      return "";
    }

    public static Object AsObject(this Field field) {
      Object ret = "";
      switch (field.Column.Type) {
        case ColumnType.Null: ret = ""; break;
        case ColumnType.String: ret = field.Value; break;
        case ColumnType.Int32: ret = field.Value.AsInt32(); break;
        case ColumnType.DateTime: ret = field.Value.AsDateTime(); break;
        case ColumnType.Boolean: ret = field.Value.AsBoolean(); break;
        case ColumnType.Decimal: ret = field.Value.AsDecimal(); break;
        case ColumnType.Byte: ret = field.Value.AsBytes(); break;
        case ColumnType.Int64: ret = field.Value.AsInt64(); break;
      }
      return ret;
    }

  }

  public class Field {
    public Row Owner { get; set; }
    public Column Column { get; set; }
    public string Value { get; set; }
    public Field(Row aRow, Column aCol, string aValue) {
      Owner = aRow;
      Column = aCol;
      Value = aValue;
    }
    public Object AsObj { 
      get { return this.AsObject(); } 
      set { this.Value = value?.AsString() ?? ""; } }
  }

  public class Row : ConcurrentDictionary<string, Field> {
    public Rows Owner;
    public long Key = 0;
    public Row(Rows aOwner) : base() {
      Owner = aOwner;
      foreach (int key in Owner.Cols.Keys.OrderBy(x => x)) {
        Column? aCol = Owner?.Cols[key] ?? null;
        if (aCol != null) {
          Field aField = Add(new Field(this, aCol, ""));
        }
      }
    }
    public virtual Boolean Contains(string Key) {
      try {  // keept for backward compatable...
        return base.ContainsKey(Key);
      } catch {
        return false;
      }
    }
    public Field Add(Field field) {
      base[field.Column.Name] = field;
      return (Field)field;
    }
    public new Field? this[string FieldName] {
      get {
        return ContainsKey(FieldName) ? (Field)base[FieldName] : null;
      }
      set {
        if (value != null) {
          base[FieldName] = value;
        } else {
          if (ContainsKey(FieldName)) {
            _ = base.TryRemove(FieldName, out _);
          }
        }
      }
    }
  }

  public class Rows : ConcurrentDictionary<long, Row> {
    public FileTable Owner;
    public Columns Cols;
    public new Row? this[long rowId] {
      get { return (ContainsKey(rowId) ? (Row)base[rowId] : null); }
      set {
        var lid = rowId;
        if (value != null) {
          if (lid == 0) {
            value.Key = GetNextId();
            lid = value.Key;
          }
          if (lid != value.Key) {
            value.Key = lid;
          }
          base[lid] = value;
        } else {
          var aKey = base[lid]?.Key ?? 0;
          if ((aKey != 0) && ContainsKey(aKey)) {
            _ = base.TryRemove(aKey, out _);
          }
        }
      }
    }
    public Rows(FileTable aOwner, Columns columns) : base() {
      Owner = aOwner;
      Cols = columns;
    }
    public virtual Boolean Contains(long rowId) {
      try {
        return base.ContainsKey(rowId);
      } catch {
        return false;
      }
    }
    public long GetNextId() {
      long max = 0;
      if (this.Keys.Count > 0) {
        max = this.Select(x => x.Value).Max(x => ((Row)x).Key);
      }
      return max + 1;
    }

    public Row Add(Row row) {
      row.Owner = this;
      if (row.Key == 0) {
        row.Key = GetNextId();
      }
      base[row.Key] = row;
      return (Row)row;
    }

    public ICollection<string> AsList {
      get {
        List<string> retList = new List<string>();
        foreach (long index in this.Keys) {
          try { 
            string encoded = this[index]?.AsEncoded() ?? "";
            if (!string.IsNullOrEmpty(encoded)) {
              retList.Add(encoded);
            }
          } catch { }
        }
        return retList;
      }
      set {
        base.Clear();
        foreach (var encoded in value) {
          try { 
            if (!string.IsNullOrEmpty(encoded)) {
              Row? row = this.AsDecoded(encoded);
              if (row != null) {
                this.Add(row);
              }
            }
          } catch { }
        }
      }
    }
  }




}