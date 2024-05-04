using System.Collections.Concurrent;
using System.Text;

namespace FileTable {

  public enum ColumnType { Null = 0, String = 1, Int32 = 2, DateTime = 3, Boolean = 4, Decimal = 5 }

  public class Column {
    public Column(Columns owner) {
      Owner = owner;
    }
    public Columns Owner { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ColumnType Type { get; set; }
  }

  public class Columns : ConcurrentDictionary<int, Column> {
    private readonly ConcurrentDictionary<string, Column> byName;
    public Columns() : base() {
      byName = new ConcurrentDictionary<string, Column>();
    }
    public virtual Boolean Contains(int index) {
      try {
        return !(base[index] is null);
      } catch {
        return false;
      }
    }
    public virtual Boolean Contains(string name) {
      try {
        return !(byName[name] is null);
      } catch {
        return false;
      }
    }
    public virtual new Column? this[int id] {
      get { return Contains(id) ? (Column)base[id] : null; }
      set {
        var lid = id;
        if (value != null) {
          if (id == 0) {
            value.Id = GetNextId();
            lid = value.Id;
          }
          if (lid != value.Id) {
            value.Id = lid;
          }
          base[lid] = value;
          byName[value.Name] = value;
        } else {
          var valName = base[id]?.Name ?? "";
          if ((valName != "") && Contains(valName)) {
            _ = byName.TryRemove(valName, out _);
          }
          if (Contains(id)) {
            _ = base.TryRemove(id, out _);
          }
        }
      }
    }

    public Column? ByName(string Name) {
      if (Name == null) return null;
      if (Contains(Name)) {
        return byName[Name];
      }
      return null;
    }

    public virtual void Remove(int id) {
      if (Contains(id)) {
        var valName = base[id]?.Name ?? "";
        if ((valName != "") && Contains(valName)) {
          _ = byName.TryRemove(valName, out _);
        }
        _ = base.TryRemove(id, out _);
      }
    }

    public int GetNextId() {
      int max = 0;
      if (this.Keys.Count > 0) {
        max = this.Select(x => x.Value).Max(x => ((Column)x).Id);
      }
      return max + 1;
    }

    public ICollection<string> AsList {
      get {
        List<string> retList = new List<string>();
        foreach (Column i in Values) {
          retList.Add(i.AsEncoded());
        }
        return retList;
      }
      set {
        base.Clear();
        foreach (var x in value) {
          Column n = new Column(this).AsDecoded(x);
          this[n.Id] = n;
        }
      }
    }
  }

  public static class ColExt {
    public static string[] Parse(this string content, string delims) {
      return content.Split(delims.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    }
    public static int AsInt(this string obj) {
      return int.TryParse(obj, out int r) ? r : 0;
    }
    public static string AsBase64Encoded(this string Text) {
      return Convert.ToBase64String(Encoding.UTF8.GetBytes(Text));
    }

    public static string AsBase64Decoded(this string Text) {
      if (string.IsNullOrEmpty(Text)) return "";
      byte[] bytes = Convert.FromBase64String(Text);
      return Encoding.UTF8.GetString(bytes);
    }

    public static string AsEncoded(this Column item) {
      return $"{item.Id} {(int)item.Type} {item.Name.AsBase64Encoded()}".AsBase64Encoded();
    }

    public static Column AsDecoded(this Column item, string encoded) {
      var arr = encoded.AsBase64Decoded().Parse(" ");
      item.Id = arr[0].AsInt();
      item.Type = (ColumnType)arr[1].AsInt();
      item.Name = arr[2].AsBase64Decoded();
      return item;
    }

  }
}