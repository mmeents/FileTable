using FileTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTableViewer.Models {
  public static class CExt {
    public const string CommonPathAdd = "\\PrompterFiles";
    public const string SettingsAdd = "\\FileTableViewerSettings.sft";
    public const string FollowedUserFileNameAdd = "\\FollowsUser.ftx";
    public const string FollowsFileName = "\\Follows.ftx";
    public static string DefaultPath {
      get {
        var DefaultDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + CExt.CommonPathAdd;
        if (!Directory.Exists(DefaultDir)) {
          Directory.CreateDirectory(DefaultDir);
        }
        return DefaultDir;
      }
    }
    public static string SettingsFileName { get { return DefaultPath + SettingsAdd; } }    
  }

  public static class Ext {
    public static StringDict AsDict(this string content, string delims) {
      var list = content.Parse(delims);
      var ret = new StringDict();
      foreach (string item in list) {
        ret.Add(item);
      }
      return ret;
    }

    public static Object AsFieldType(this Field field) {
      Object ret = "";          
      switch (field.Column.Type) {
        case ColumnType.Null: ret =""; break;
        case ColumnType.String: ret = field.Value; break;
        case ColumnType.Int32: ret = field.Value.AsInt32(); break;
        case ColumnType.DateTime: ret = field.Value.AsDateTime(); break;
        case ColumnType.Boolean: ret = field.Value.AsBoolean(); break;
        case ColumnType.Decimal: ret = field.Value.AsDecimal(); break;
        case ColumnType.Byte: ret = "(ByteArrayUnsupported.)"; break;
        case ColumnType.Int64: ret = field.Value.AsInt64(); break;
      }
      return ret;        
    }

  }
}
