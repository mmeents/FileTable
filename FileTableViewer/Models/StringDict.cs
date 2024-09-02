using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTableViewer.Models {
  public class StringDict : ConcurrentDictionary<string, string> {
    
    public virtual string Add(string value) {
      if (!ContainsKey(value)) {
        TryAdd(value, value);
      }
      return value;
    }

    public string AsString() {
      StringBuilder ret = new StringBuilder();
      foreach (string key in base.Keys) {
        ret.Append(key + Environment.NewLine);
      }
      return ret.ToString();
    }

  }
}
