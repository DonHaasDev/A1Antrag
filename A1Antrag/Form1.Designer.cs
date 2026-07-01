#nullable enable
using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace A1Antrag;

partial class Form1
{
    private IContainer? components = null;

    private FlowLayoutPanel btnPanel = null!;
    private Button btnNeu = null!;
    private Button btnBearbeiten = null!;
    private Button btnStornieren = null!;
    private Button btnLoeschen = null!;
    private Button btnBeantragen = null!;
    private Button btnGenehmigen = null!;
    private Button btnVorlErhebung = null!;
    private Button btnAktualisieren = null!;

    private GroupControl grpFilter = null!;
    private RadioGroup radioGroup1 = null!;

    private GridControl gridControl1 = null!;
    private GridView gridView1 = null!;

    private StatusStrip statusStrip1 = null!;
    private ToolStripStatusLabel statusLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();

        btnPanel         = new FlowLayoutPanel();
        grpFilter        = new GroupControl();
        radioGroup1      = new RadioGroup();
        gridControl1     = new GridControl();
        gridView1        = new GridView();
        statusStrip1     = new StatusStrip();
        statusLabel      = new ToolStripStatusLabel();

        btnPanel.SuspendLayout();
        ((ISupportInitialize)grpFilter).BeginInit();
        grpFilter.SuspendLayout();
        ((ISupportInitialize)radioGroup1.Properties).BeginInit();
        ((ISupportInitialize)gridControl1).BeginInit();
        ((ISupportInitialize)gridView1).BeginInit();
        statusStrip1.SuspendLayout();
        SuspendLayout();

        // ── Button Panel ─────────────────────────────────────────────
        btnPanel.Dock          = DockStyle.Top;
        btnPanel.Height        = 34;
        btnPanel.BackColor     = Color.FromArgb(30, 30, 30);
        btnPanel.FlowDirection = FlowDirection.LeftToRight;
        btnPanel.WrapContents  = false;
        btnPanel.Padding       = new Padding(0);
        btnPanel.Margin        = new Padding(0);

        var btnFont = new Font("Segoe UI", 9F, FontStyle.Bold);

        Button MakeBtn(string text, Color backColor, EventHandler handler, bool visible = true)
        {
            var btn = new Button
            {
                Text                   = text,
                FlatStyle              = FlatStyle.Flat,
                BackColor              = backColor,
                ForeColor              = Color.White,
                Font                   = btnFont,
                Height                 = 34,
                Width                  = TextRenderer.MeasureText(text, btnFont).Width + 24,
                UseVisualStyleBackColor = false,
                Cursor                 = Cursors.Hand,
                Visible                = visible,
                Margin                 = new Padding(1, 0, 0, 0),
                Padding                = new Padding(0),
                TabStop                = false
            };
            btn.FlatAppearance.BorderSize         = 0;
            btn.FlatAppearance.MouseOverBackColor  = Color.FromArgb(
                Math.Min(255, backColor.R + 30),
                Math.Min(255, backColor.G + 30),
                Math.Min(255, backColor.B + 30));
            btn.FlatAppearance.MouseDownBackColor  = Color.FromArgb(
                Math.Max(0, backColor.R - 20),
                Math.Max(0, backColor.G - 20),
                Math.Max(0, backColor.B - 20));
            btn.Click += handler;
            btnPanel.Controls.Add(btn);
            return btn;
        }

        btnNeu           = MakeBtn("Neu",                Color.FromArgb( 41,  98, 255), (_, _) => btnNeu_Click(this, EventArgs.Empty));
        btnBearbeiten    = MakeBtn("Bearbeiten",          Color.FromArgb( 30,  60, 168), (_, _) => btnBearbeiten_Click(this, EventArgs.Empty));
        btnStornieren    = MakeBtn("Stornieren",          Color.FromArgb(183,  28,  28), (_, _) => btnStornieren_Click(this, EventArgs.Empty));
        btnLoeschen      = MakeBtn("Löschen",             Color.FromArgb(183,  28,  28), (_, _) => btnLoeschen_Click(this, EventArgs.Empty),    visible: false);
        btnBeantragen    = MakeBtn("Beantragen",          Color.FromArgb(230,  81,   0), (_, _) => btnBeantragen_Click(this, EventArgs.Empty),   visible: false);
        btnGenehmigen    = MakeBtn("Genehmigen",          Color.FromArgb( 27,  94,  32), (_, _) => btnGenehmigen_Click(this, EventArgs.Empty),   visible: false);
        btnVorlErhebung  = MakeBtn("Vorl. Erhebung",      Color.FromArgb( 13,  71, 161), (_, _) => btnVorlErhebung_Click(this, EventArgs.Empty), visible: false);
        btnAktualisieren = MakeBtn("Aktualisieren (F5)",  Color.FromArgb( 66,  66,  66), (_, _) => btnAktualisieren_Click(this, EventArgs.Empty));

        // ── Filter (GroupControl + RadioGroup) ───────────────────────
        grpFilter.Text   = "Filter";
        grpFilter.Dock   = DockStyle.Top;
        grpFilter.Height = 56;
        grpFilter.Controls.Add(radioGroup1);

        radioGroup1.Dock = DockStyle.Fill;
        radioGroup1.Properties.Appearance.BackColor = Color.Transparent;
        radioGroup1.Properties.Appearance.Options.UseBackColor = true;
        radioGroup1.Properties.Columns = 2;
        radioGroup1.Properties.Items.AddRange(new RadioGroupItem[] {
            new RadioGroupItem(0, "Offene Anträge anzeigen"),
            new RadioGroupItem(1, "Alle Anträge anzeigen (bis zu 2 Jahre zurück)")
        });
        radioGroup1.EditValue = 0;
        radioGroup1.SelectedIndexChanged += (_, _) => LoadData();

        // ── GridControl / GridView ───────────────────────────────────
        gridControl1.Dock           = DockStyle.Fill;
        gridControl1.MainView       = gridView1;
        gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });

        gridView1.GridControl                             = gridControl1;
        gridView1.Name                                    = "gridView1";
        gridView1.OptionsView.ShowGroupPanel              = true;
        gridView1.OptionsView.ColumnAutoWidth             = false;
        gridView1.OptionsView.EnableAppearanceEvenRow     = true;
        gridView1.OptionsFind.AlwaysVisible               = true;
        gridView1.OptionsFind.FindMode                    = DevExpress.XtraEditors.FindMode.Always;
        gridView1.OptionsBehavior.Editable                = false;
        gridView1.OptionsSelection.MultiSelect            = false;
        gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
        gridView1.FocusRectStyle                          = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
        gridView1.RowStyle    += gridView1_RowStyle;
        gridView1.DoubleClick += gridView1_DoubleClick;

        // ── StatusStrip ──────────────────────────────────────────────
        statusStrip1.Items.AddRange(new ToolStripItem[] { statusLabel });
        statusStrip1.BackColor = Color.FromArgb(240, 240, 240);
        statusLabel.Text      = "Verbinde …";
        statusLabel.Spring    = true;
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;

        // ── Form ─────────────────────────────────────────────────────
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(1280, 720);
        MinimumSize         = new Size(900, 500);

        // Add order determines dock stacking: last added Top = topmost
        Controls.Add(gridControl1);   // Fill
        Controls.Add(grpFilter);      // Top (below button bar)
        Controls.Add(btnPanel);       // Top (topmost)
        Controls.Add(statusStrip1);   // Bottom

        Font       = new Font("Segoe UI", 9F);
        Name       = "Form1";
        Text       = "A1-Antrag Verwaltung – Servolift";
        KeyPreview = true;
        Load      += Form1_Load;
        KeyDown   += Form1_KeyDown;

        ((ISupportInitialize)radioGroup1.Properties).EndInit();
        ((ISupportInitialize)grpFilter).EndInit();
        grpFilter.ResumeLayout(false);
        ((ISupportInitialize)gridView1).EndInit();
        ((ISupportInitialize)gridControl1).EndInit();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        btnPanel.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
