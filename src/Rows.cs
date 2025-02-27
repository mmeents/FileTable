using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTables {
  public class Rows : ConcurrentDictionary<int, RowModel> {
    private readonly object _lock = new();
    private readonly FileTable _owner;
    public Rows(FileTable owner) : base() {
      _owner = owner;
    }
    public Rows(FileTable owner, IEnumerable<RowModel> rows) : base() {
      _owner = owner;
      AsList = rows;
    }
    public virtual new RowModel this[int id] {
      get {
        return base[id];
      }
      set {
        lock (_lock) {
          if (value != null) {
            Add(value);
          } else {
            Remove(id);
          }
        }
      }
    }
    public int GetNextId() {
      int max = 0;
      if (this.Keys.Count > 0) {
        max = this.Select(x => x.Value).Max(x => x.Id);
      }
      return max + 1;
    }
    public RowModel Add(RowModel row) {
      lock (_lock) {        
        if (row.Id == 0) {
          row.Id = GetNextId();
        }
        base[row.Id] = row;
        row.Owner = _owner;
        row.RowFields = new Fields( _owner.GetFieldsOfRow(row.Id), _owner.Columns);
        return row;
      }
    }
    public void Remove(int id) {
      lock (_lock) {
        
        if (ContainsKey(id)) {
          _ = base.TryRemove(id, out _);
        }
      }
    }
    public void Remove(RowModel row) {
      lock (_lock) {
        if (row == null) return;
        if (ContainsKey(row.Id)) {
          _ = base.TryRemove(row.Id, out _);
        }
      }
    }
    public IEnumerable<RowModel> AsList {
      get {
        return this.Select(x => x.Value);
      }
      set {
        lock (_lock) {
          if (value == null) return;
          foreach (var item in value) {
            Add(item);
          }
        }
      }
    }

  }
}
