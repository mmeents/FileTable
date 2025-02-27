using System.Collections.Concurrent;
using System.Data.Common;
using System.Text;

namespace FileTables
{
  public class Columns : ConcurrentDictionary<int, ColumnModel> {
    private readonly object _lock = new object();
    private readonly ConcurrentDictionary<string, ColumnModel> byName;
    public Columns() : base() {
      byName = new ConcurrentDictionary<string, ColumnModel>();
    }
    public Columns(IEnumerable<ColumnModel> columns) : base() {
      byName = new ConcurrentDictionary<string, ColumnModel>();
      AsList = columns;
    }
    public virtual Boolean Contains(int index) {
      lock (_lock) {
        try {
          return base.ContainsKey(index);
        } catch {
          return false;
        }
      }
    }
    public virtual Boolean Contains(string ColumnName) {
      lock (_lock) {
        try {
          return byName.ContainsKey(ColumnName);
        } catch {
          return false;
        }
      }
    }
    public virtual new ColumnModel? this[int id] {
      get {
        lock (_lock) {
          return Contains(id) ? (ColumnModel)base[id] : null; 
        }
      }
      set {
        var lid = id;
        lock (_lock) {
          if (value != null) {
            if (id == 0) {
              value.Id = GetNextId();
              lid = value.Id;
            }
            if (lid != value.Id) {
              value.Id = lid;
            }
            base[lid] = value;
            byName[value.ColumnName] = value;
          } else {
            var valName = base[id]?.ColumnName ?? "";
            if ((valName != "") && Contains(valName)) {
              _ = byName.TryRemove(valName, out _);
            }
            if (Contains(id)) {
              _ = base.TryRemove(id, out _);
            }
          }
        }
      }
    }

    public ColumnModel? ByName(string Name) {
      if (Name == null) return null;
      if (Contains(Name)) {
        return byName[Name];
      }
      return null;
    }

    public virtual void Remove(int id) {
      if (Contains(id)) {
        var valName = base[id]?.ColumnName ?? "";
        if ((valName != "") && Contains(valName)) {
          _ = byName.TryRemove(valName, out _);
        }
        _ = base.TryRemove(id, out _);
      }
    }

    public int GetNextId() {
      int max = 0;
      if (this.Keys.Count > 0) {
        max = this.Select(x => x.Value).Max(x => ((ColumnModel)x).Id);
      }
      return max + 1;
    }

    public ColumnModel Add(ColumnModel column) { 
      lock (_lock) { 
        if (column.Id == 0) { 
          column.Id = GetNextId();
        }
        base[column.Id] = column;
        byName[column.ColumnName] = column;
        return column;
      }
    }

    public IEnumerable<ColumnModel> AsList {
      get {
        lock (_lock) { 
          return base.Values.ToList().OrderBy(x => x.Rank);
        }
      }
      set {
        base.Clear();
        byName.Clear();
        foreach (var x in value) {
          Add(x);
        }
      }
    }
  }
  
}
