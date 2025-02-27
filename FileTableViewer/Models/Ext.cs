using FileTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTableViewer.Models {
  public static class CExt {
    public const string CommonPathAdd = "\\PrompterFiles";
    public const string SettingsAdd = "\\FileTableViewerSettings2.sft";
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

  }
}
