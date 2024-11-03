using System;
using System.Collections.Concurrent;
using System.Text;


namespace FileTables {
  public class Row : ConcurrentDictionary<string, Field> {

    public Rows Owner { get; set; }
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
      return field;
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


  public static class RowExt {

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



  }
}
