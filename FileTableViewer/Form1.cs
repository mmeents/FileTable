using FileTables;
using FileTables.Interfaces;
using System;
using static System.Net.Mime.MediaTypeNames;
using FileTableViewer.Models;
using System.Linq;
using System.DirectoryServices.ActiveDirectory;
using System.Collections.Concurrent;
using Microsoft.VisualBasic.Logging;

namespace FileTableViewer {
  public partial class Form1 : Form, ILogMsg {

    private SettingsFile _settingsPack;
    private Settings _settings;
    private FileTable? _table;
    private bool _tableDirty = false;
    public bool TableDirty {
      get { return _tableDirty; }
      set { _tableDirty = value; SetSaveCancelState(); }
    }

    private ConcurrentDictionary<int, int> _UiToTableIndex = new ConcurrentDictionary<int, int>();
    private string _defaultDir = "";


    private string _fileName = "No File Open";
    private string _folder;
    private string OrderByColumnName = "";
    private bool SortAsc = true;

    public Form1() {
      InitializeComponent();
      scLvl0.Panel2Collapsed = true;
      vrMain.Visible = false;
      toolStrip1.Visible = false;

      _defaultDir = CExt.DefaultPath;
      _settingsPack = new SettingsFile(CExt.SettingsFileName, this);
      _settings = _settingsPack.Settings;

      string smrul = _settings["MRUL"].Value;
      if (!String.IsNullOrEmpty(smrul)) {
        comboBox1.Items.Clear();
        comboBox1.Items.AddRange(smrul.Parse(Environment.NewLine));
        comboBox1.SelectedIndex = 0;
      }
    }

    delegate void SetLogMsgCallback(string msg);
    public void LogMsg(string msg) {
      if (this.TextErrorLog.InvokeRequired) {
        SetLogMsgCallback d = new(LogMsg);
        this.BeginInvoke(d, new object[] { msg });
      } else {
        if (scLvl0.Panel2Collapsed) { scLvl0.Panel2Collapsed = false; }
        if (!TextErrorLog.Visible) TextErrorLog.Visible = true;
        this.TextErrorLog.Text = msg + Environment.NewLine + TextErrorLog.Text;
      }
    }
    private void HideErrorPanel_Click(object sender, EventArgs e) {
      if (!scLvl0.Panel2Collapsed) {
        scLvl0.Panel2Collapsed = true;
      }
    }


    private void AddFileToMRUL(string fileName) {
      if (_settings != null) {
        if (!_settings.ContainsKey("MRUL")) {
          _settings["MRUL"] = new SettingProperty() { Key = "MRUL", Value = fileName };
        } else {
          var mrul = _settings["MRUL"].Value.Parse(Environment.NewLine);
          string newMRUL = (mrul.Length > 0 ? mrul[0] : "")
            + (mrul.Length > 1 ? Environment.NewLine + mrul[1] : "")
            + (mrul.Length > 2 ? Environment.NewLine + mrul[2] : "");
          StringDict mruld = newMRUL.AsDict(Environment.NewLine);
          mruld.Add(fileName);
          _settings["MRUL"].Value = fileName + Environment.NewLine + mruld.AsString();
        }
      }
    }

    private void btnBrowse_Click(object sender, EventArgs e) {
      if (_fileName == "No File Open") {
        odMain.InitialDirectory = _defaultDir;
      } else {
        string s = _fileName.ParseLast("\\");
        odMain.InitialDirectory = _fileName.Substring(0, _fileName.Length - s.Length);
      }
      DialogResult res = odMain.ShowDialog();
      if (res == DialogResult.OK) {
        _fileName = odMain.FileName;
        DoOpenFileTable();
      }
    }

    private void btnOpenClose_Click(object sender, EventArgs e) {
      if (btnOpenClose.Text != "Open") {
        DoCloseFileTable();
      } else {
        try {
          _fileName = comboBox1.SelectedItem?.ToString() ?? comboBox1.Text;
          DoOpenFileTable();
        } catch (Exception ex) {
          LogMsg(ex.Message);
        }
      }
    }


    public void DoLoadVrMain() {
      _table.Active = false;
      _table.Active = true;

      if (vrMain.Rows.Count > 0) { vrMain.Rows.Clear(); }
      if (vrMain.Columns.Count > 0) { vrMain.Columns.Clear(); }
      if (!toolStrip1.Visible) { toolStrip1.Visible = true; }
      TableDirty = false;

      foreach (var col in _table.Columns.Values.OrderBy(x => x.Rank)) {
        var addedId = vrMain.Columns.Add(col.ColumnName, col.ColumnName);
        if (col.ColumnName == OrderByColumnName) {
          if (SortAsc) {
            vrMain.Columns[addedId].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
          } else {
            vrMain.Columns[addedId].HeaderCell.SortGlyphDirection = SortOrder.Descending;
          }
        } else {
          vrMain.Columns[addedId].HeaderCell.SortGlyphDirection = SortOrder.None;
        }
      }
      vrMain.RowCount = _table.Rows.Count;
      if (OrderByColumnName == "") {
        IOrderedEnumerable<int> listRowKeys;
        if (SortAsc) {
          listRowKeys = _table.Rows.Keys.OrderBy(x => x);
        } else {
          listRowKeys = _table.Rows.Keys.OrderByDescending(x => x);
        }
        var xRow = 0;
        foreach (var key in listRowKeys) {
          _UiToTableIndex[xRow] = key;
          xRow++;
        }
      } else {
        string IndexName = vrMain.Columns[0].Name;
        IOrderedEnumerable<RowModel> listRows;
        if (SortAsc) {
          listRows = _table.Rows.Select(x => x.Value).OrderBy(x => x[OrderByColumnName].Value);
        } else {
          listRows = _table.Rows.Select(x => x.Value).OrderByDescending(x => x[OrderByColumnName].Value);
        }
        var xRow = 0;
        foreach (var row in listRows) {
          int key = row[IndexName].AsInt32();
          _UiToTableIndex[xRow] = key;
          xRow++;
        }
      }
      if (!vrMain.Visible) { vrMain.Visible = true; }
    }

    public void DoOpenFileTable() {
      try {
        this.Text = $"Viewing {_fileName}";
        _folder = _fileName.Substring(0, _fileName.Length - (_fileName.ParseLast("\\").Length + 1));
        if (!Directory.Exists(_folder)) {
          Directory.CreateDirectory(_folder);
        }
        _table = new FileTable(_fileName);
        if (!_table.Active) { _table.Active = true; }
        if (_table.Columns.Count == 0) { LogMsg("No columns in file.  Aborting open."); return; }
        DoLoadVrMain();
        AddFileToMRUL(_fileName);
        _settingsPack.Settings = _settings;
        _settingsPack.Save();
        btnOpenClose.Text = "Close";
        comboBox1.Visible = false;
        btnBrowse.Visible = false;
        label1.Visible = false;
      } catch (Exception ex) {
        LogMsg(ex.Message);
      }
    }

    private void DoCloseFileTable() {
      vrMain.Visible = false;
      toolStrip1.Visible = false;
      this.Text = "FileTable Viewer";
      btnOpenClose.Text = "Open";
      comboBox1.Visible = true;
      label1.Visible = true;
      btnBrowse.Visible = true;
    }

    private void vrMain_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e) {
      var columnName = vrMain.Columns[e.ColumnIndex].Name;
      var tblIndex = _UiToTableIndex[e.RowIndex]; // todo on refresh, index is missing new rows..
      e.Value = _table.Rows[tblIndex]?[columnName]?.Value ?? "";
    }

    private void vrMain_CellValuePushed(object sender, DataGridViewCellValueEventArgs e) {
      var columnName = vrMain.Columns[e.ColumnIndex].Name;
      var tblIndex = _UiToTableIndex[e.RowIndex];
      _table.Rows[tblIndex][columnName].Value = e.Value ?? "";
      TableDirty = true;
    }

    private void vrMain_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e) {

    }

    private void vrMain_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
      var columnName = vrMain.Columns[e.ColumnIndex].Name;
      if (columnName == OrderByColumnName) {
        SortAsc = !SortAsc;
        var col = vrMain.Columns[e.ColumnIndex];
      } else {
        OrderByColumnName = columnName;
      }
      DoLoadVrMain();
    }

    private void SetSaveCancelState() {
      if (_tableDirty) {
        if (!btnOK.Enabled) btnOK.Enabled = true;
        if (!btnCancel.Enabled) btnCancel.Enabled = true;
        lbStatus.Text = "Status: Updated ";
      } else {
        if (btnOK.Enabled) btnOK.Enabled = false;
        if (btnCancel.Enabled) btnCancel.Enabled = false;
        lbStatus.Text = "Status: Browse ";
      }
    }
    private void btnOK_Click(object sender, EventArgs e) {
      if (_table != null) {
        _table.SaveToFile();
        TableDirty = false;
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) {
      if (_table != null) {
        _table.Active = false;
        _table.Active = true;
        DoLoadVrMain();
      }
    }

    private void btnRefresh_Click(object sender, EventArgs e) {
      if (_table != null) {
        _table.Active = false;
        _table.Active = true;
        DoLoadVrMain();
      }      
    }
  }
}