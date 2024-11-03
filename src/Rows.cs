using MessagePack;
using System.Collections.Concurrent;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace FileTables {
  
  

  public class Rows : ConcurrentDictionary<long, Row> {
    public FileTable Owner;
    public Columns Cols;
    public Rows(FileTable aOwner, Columns columns) : base() {
      Owner = aOwner;
      Cols = columns;
    }

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

    public void Remove(long rowId) {
      if (Contains(rowId)) {
        _ = base.TryRemove(rowId, out _);
      }
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