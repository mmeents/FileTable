using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTables
{
  public class StringDict : ConcurrentDictionary<string, string> {
    public virtual Boolean Contains(String key) {
      try {
        return (!(base[key] is null));
      } catch {
        return false;
      }
    }
    public virtual string Add(string value) {
      if (!Contains(value)) {
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

  public static class StringDictExt {
    public static StringDict AsDict(this string content, string delims) {
      var list = content.Parse(delims);
      var ret = new StringDict();
      foreach (string item in list) {
        ret.Add(item);
      }
      return ret;
    }
  }
}
