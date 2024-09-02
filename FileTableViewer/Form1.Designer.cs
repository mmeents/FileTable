namespace FileTableViewer {
  partial class Form1 {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      scLvl0 = new SplitContainer();
      panel2 = new Panel();
      vrMain = new DataGridView();
      panel1 = new Panel();
      toolStrip1 = new ToolStrip();
      lbStatus = new ToolStripLabel();
      btnOK = new ToolStripButton();
      btnCancel = new ToolStripButton();
      label1 = new Label();
      btnBrowse = new Button();
      btnOpenClose = new Button();
      comboBox1 = new ComboBox();
      LabelErrorCaption = new Label();
      TextErrorLog = new TextBox();
      HideErrorPanel = new Button();
      odMain = new OpenFileDialog();
      ((System.ComponentModel.ISupportInitialize)scLvl0).BeginInit();
      scLvl0.Panel1.SuspendLayout();
      scLvl0.Panel2.SuspendLayout();
      scLvl0.SuspendLayout();
      panel2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)vrMain).BeginInit();
      panel1.SuspendLayout();
      toolStrip1.SuspendLayout();
      SuspendLayout();
      // 
      // scLvl0
      // 
      scLvl0.Dock = DockStyle.Fill;
      scLvl0.Location = new Point(0, 0);
      scLvl0.Name = "scLvl0";
      scLvl0.Orientation = Orientation.Horizontal;
      // 
      // scLvl0.Panel1
      // 
      scLvl0.Panel1.Controls.Add(panel2);
      scLvl0.Panel1.Controls.Add(panel1);
      // 
      // scLvl0.Panel2
      // 
      scLvl0.Panel2.Controls.Add(LabelErrorCaption);
      scLvl0.Panel2.Controls.Add(TextErrorLog);
      scLvl0.Panel2.Controls.Add(HideErrorPanel);
      scLvl0.Size = new Size(872, 516);
      scLvl0.SplitterDistance = 304;
      scLvl0.TabIndex = 0;
      // 
      // panel2
      // 
      panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      panel2.Controls.Add(vrMain);
      panel2.Location = new Point(0, 84);
      panel2.Name = "panel2";
      panel2.Size = new Size(867, 213);
      panel2.TabIndex = 1;
      // 
      // vrMain
      // 
      vrMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
      vrMain.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
      vrMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = SystemColors.Window;
      dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
      dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
      dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
      vrMain.DefaultCellStyle = dataGridViewCellStyle1;
      vrMain.Dock = DockStyle.Fill;
      vrMain.Location = new Point(0, 0);
      vrMain.Name = "vrMain";
      vrMain.RowHeadersWidth = 51;
      vrMain.RowTemplate.Height = 29;
      vrMain.Size = new Size(867, 213);
      vrMain.TabIndex = 0;
      vrMain.VirtualMode = true;
      vrMain.CellValueNeeded += vrMain_CellValueNeeded;
      vrMain.CellValuePushed += vrMain_CellValuePushed;
      vrMain.ColumnHeaderMouseClick += vrMain_ColumnHeaderMouseClick;
      // 
      // panel1
      // 
      panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      panel1.Controls.Add(toolStrip1);
      panel1.Controls.Add(label1);
      panel1.Controls.Add(btnBrowse);
      panel1.Controls.Add(btnOpenClose);
      panel1.Controls.Add(comboBox1);
      panel1.Dock = DockStyle.Top;
      panel1.Location = new Point(0, 0);
      panel1.Name = "panel1";
      panel1.Size = new Size(872, 78);
      panel1.TabIndex = 0;
      // 
      // toolStrip1
      // 
      toolStrip1.Dock = DockStyle.Bottom;
      toolStrip1.ImageScalingSize = new Size(20, 20);
      toolStrip1.Items.AddRange(new ToolStripItem[] { lbStatus, btnOK, btnCancel });
      toolStrip1.Location = new Point(0, 51);
      toolStrip1.Name = "toolStrip1";
      toolStrip1.Size = new Size(872, 27);
      toolStrip1.TabIndex = 19;
      toolStrip1.Text = "toolStrip1";
      // 
      // lbStatus
      // 
      lbStatus.Name = "lbStatus";
      lbStatus.Size = new Size(108, 24);
      lbStatus.Text = "Status: Browse ";
      // 
      // btnOK
      // 
      btnOK.DisplayStyle = ToolStripItemDisplayStyle.Image;
      btnOK.Enabled = false;
      btnOK.Image = (Image)resources.GetObject("btnOK.Image");
      btnOK.ImageTransparentColor = Color.Magenta;
      btnOK.Name = "btnOK";
      btnOK.Size = new Size(29, 24);
      btnOK.Text = "toolStripButton1";
      btnOK.ToolTipText = "Save Updates";
      btnOK.Click += btnOK_Click;
      // 
      // btnCancel
      // 
      btnCancel.Checked = true;
      btnCancel.CheckState = CheckState.Indeterminate;
      btnCancel.DisplayStyle = ToolStripItemDisplayStyle.Image;
      btnCancel.Enabled = false;
      btnCancel.Image = (Image)resources.GetObject("btnCancel.Image");
      btnCancel.ImageTransparentColor = Color.Magenta;
      btnCancel.Name = "btnCancel";
      btnCancel.Size = new Size(29, 24);
      btnCancel.Text = "toolStripButton2";
      btnCancel.ToolTipText = "Cancel Update and reload";
      btnCancel.Click += btnCancel_Click;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(20, 16);
      label1.Name = "label1";
      label1.Size = new Size(110, 20);
      label1.TabIndex = 18;
      label1.Text = "Open FileTable:";
      // 
      // btnBrowse
      // 
      btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btnBrowse.Location = new Point(716, 13);
      btnBrowse.Name = "btnBrowse";
      btnBrowse.Size = new Size(69, 27);
      btnBrowse.TabIndex = 17;
      btnBrowse.Text = "Browse";
      btnBrowse.UseVisualStyleBackColor = true;
      btnBrowse.Click += btnBrowse_Click;
      // 
      // btnOpenClose
      // 
      btnOpenClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btnOpenClose.Location = new Point(791, 13);
      btnOpenClose.Name = "btnOpenClose";
      btnOpenClose.Size = new Size(69, 27);
      btnOpenClose.TabIndex = 16;
      btnOpenClose.Text = "Open";
      btnOpenClose.UseVisualStyleBackColor = true;
      btnOpenClose.Click += btnOpenClose_Click;
      // 
      // comboBox1
      // 
      comboBox1.FormattingEnabled = true;
      comboBox1.Location = new Point(140, 13);
      comboBox1.Name = "comboBox1";
      comboBox1.Size = new Size(570, 28);
      comboBox1.TabIndex = 0;
      // 
      // LabelErrorCaption
      // 
      LabelErrorCaption.AutoSize = true;
      LabelErrorCaption.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
      LabelErrorCaption.Location = new Point(8, 14);
      LabelErrorCaption.Name = "LabelErrorCaption";
      LabelErrorCaption.Size = new Size(93, 25);
      LabelErrorCaption.TabIndex = 17;
      LabelErrorCaption.Text = "Errors Log";
      // 
      // TextErrorLog
      // 
      TextErrorLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      TextErrorLog.Location = new Point(12, 42);
      TextErrorLog.Multiline = true;
      TextErrorLog.Name = "TextErrorLog";
      TextErrorLog.Size = new Size(849, 155);
      TextErrorLog.TabIndex = 16;
      // 
      // HideErrorPanel
      // 
      HideErrorPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      HideErrorPanel.Location = new Point(791, 9);
      HideErrorPanel.Name = "HideErrorPanel";
      HideErrorPanel.Size = new Size(69, 27);
      HideErrorPanel.TabIndex = 15;
      HideErrorPanel.Text = "Hide";
      HideErrorPanel.UseVisualStyleBackColor = true;
      HideErrorPanel.Click += HideErrorPanel_Click;
      // 
      // odMain
      // 
      odMain.CheckFileExists = false;
      odMain.DefaultExt = "as4m";
      odMain.Filter = "AppSmith4Model|*.as4m|All files|*.*";
      odMain.Title = "Open Archinve";
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
      AutoSizeMode = AutoSizeMode.GrowAndShrink;
      ClientSize = new Size(872, 516);
      Controls.Add(scLvl0);
      Icon = (Icon)resources.GetObject("$this.Icon");
      Name = "Form1";
      Text = "FileTable Viewer";
      scLvl0.Panel1.ResumeLayout(false);
      scLvl0.Panel2.ResumeLayout(false);
      scLvl0.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)scLvl0).EndInit();
      scLvl0.ResumeLayout(false);
      panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)vrMain).EndInit();
      panel1.ResumeLayout(false);
      panel1.PerformLayout();
      toolStrip1.ResumeLayout(false);
      toolStrip1.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    private SplitContainer scLvl0;
    private Label LabelErrorCaption;
    public TextBox TextErrorLog;
    private Button HideErrorPanel;
    private Panel panel2;
    private Panel panel1;
    private DataGridView vrMain;
    private ComboBox comboBox1;
    private Label label1;
    private Button btnBrowse;
    private Button btnOpenClose;
    private OpenFileDialog odMain;
    private ToolStrip toolStrip1;
    private ToolStripLabel lbStatus;
    private ToolStripButton btnOK;
    private ToolStripButton btnCancel;
  }
}