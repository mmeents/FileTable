using MessagePack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTables.Interfaces;


namespace FileTables {

  [MessagePackObject]
  public class SettingProperty {
    [Key(0)]
    public string Key { get; set; } = "";
    [Key(1)]
    public string Value { get; set; } = "";
  }

  [MessagePackObject]
  public class SettingsPackage {
    [Key(0)]
    public string FileName { get; set; } = "";
    [Key(1)]
    public List<SettingProperty> SettingsList { get; set; } = new List<SettingProperty>();
  }

  public class Settings : ConcurrentDictionary<string, SettingProperty> {
    public Settings() : base() { }

    public Settings(ICollection<SettingProperty> asList) : base() {
      AsList = asList;
    }

    public virtual new SettingProperty this[string key] {
      get {
        if (!base.ContainsKey(key)) base[key] = new SettingProperty() { Key = key, Value = "" };
        return base[key];
      }
      set { if (value != null) { base[key] = value; } else { Remove(key); } }
    }

    public virtual void Remove(string key) {
      if (base.ContainsKey(key)) { _ = base.TryRemove(key, out _); }
    }

    public ICollection<SettingProperty> AsList {
      get { return base.Values; }
      set {
        base.Clear();
        foreach (var x in value) {
          this[x.Key] = x;
        }
      }
    }

    public Settings Clone() {
      var clone = new Settings {
        AsList = AsList
      };
      return clone;
    }
  }

 

  public class SettingsFile {
    private readonly ILogMsg _form1;

    public SettingsFile(string fileName, ILogMsg form1) {
      _form1 = form1;
      FileName = fileName;

      Package = new SettingsPackage {
        FileName = FileName
      };
      if (File.Exists(FileName)) {
        Load();
      }
    }

    public string FileName { get; set; }
    private bool _FileLoaded = false;
    public bool FileLoaded { get { return _FileLoaded; } }
    public SettingsPackage Package { get; set; }

    private Settings GetSettingsFromPackage() {
      Settings n = new Settings();
      foreach (var item in Package.SettingsList) {
        n[item.Key] = item;
      }
      return n;
    }
    private void SetSettingsToPackage(Settings value) {
      Package.SettingsList = value.AsList.ToList();
    }

    public Settings Settings { get { return GetSettingsFromPackage(); } set { SetSettingsToPackage(value); } }

    public void Load() {
      try {
        Task.Run(async () => await this.LoadAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
      } catch (Exception ex) {
        _form1.LogMsg($"Load {FileName} Error:" + ex.Message);
      }
    }
    public async Task LoadAsync() {
      if (File.Exists(FileName)) {
        var encoded = await FileName.ReadAllTextAsync();
        var decoded = Convert.FromBase64String(encoded.Replace('?', '='));
        this.Package = MessagePackSerializer.Deserialize<SettingsPackage>(decoded);
        _FileLoaded = true;
      }
    }

    public void Save() {
      try {
        Task.Run(async () => await this.SaveAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
      } catch (Exception ex) {
        _form1.LogMsg($"Save {FileName} Error:" + ex.Message);
      }
    }
    public async Task SaveAsync() {
      byte[] WirePacked = MessagePackSerializer.Serialize(this.Package);
      string encoded = Convert.ToBase64String(WirePacked);
      await encoded.WriteAllTextAsync(FileName);
    }
  }

}
