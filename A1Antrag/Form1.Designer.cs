#nullable enable
using System.ComponentModel;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace A1Antrag;

partial class Form1
{
    private IContainer? components = null;

    private BarManager barManager1 = null!;
    private Bar bar1 = null!;
    private BarDockControl barDockControlTop = null!;
    private BarDockControl barDockControlBottom = null!;
    private BarDockControl barDockControlLeft = null!;
    private BarDockControl barDockControlRight = null!;
    private BarButtonItem btnNeu = null!;
    private BarButtonItem btnBearbeiten = null!;
    private BarButtonItem btnLoeschen = null!;
    private BarButtonItem btnBeantragen = null!;
    private BarButtonItem btnGenehmigen = null!;
    private BarButtonItem btnVorlErhebung = null!;
    private BarButtonItem btnAktualisieren = null!;

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

        barManager1          = new BarManager(components);
        bar1                 = new Bar();
        barDockControlTop    = new BarDockControl();
        barDockControlBottom = new BarDockControl();
        barDockControlLeft   = new BarDockControl();
        barDockControlRight  = new BarDockControl();
        btnNeu               = new BarButtonItem();
        btnBearbeiten        = new BarButtonItem();
        btnLoeschen          = new BarButtonItem();
        btnBeantragen        = new BarButtonItem();
        btnGenehmigen        = new BarButtonItem();
        btnVorlErhebung      = new BarButtonItem();
        btnAktualisieren     = new BarButtonItem();

        grpFilter    = new GroupControl();
        radioGroup1  = new RadioGroup();

        gridControl1 = new GridControl();
        gridView1    = new GridView();

        statusStrip1 = new StatusStrip();
        statusLabel  = new ToolStripStatusLabel();

        ((ISupportInitialize)barManager1).BeginInit();
        ((ISupportInitialize)grpFilter).BeginInit();
        grpFilter.SuspendLayout();
        ((ISupportInitialize)radioGroup1.Properties).BeginInit();
        ((ISupportInitialize)gridControl1).BeginInit();
        ((ISupportInitialize)gridView1).BeginInit();
        statusStrip1.SuspendLayout();
        SuspendLayout();

        // ── BarManager ───────────────────────────────────────────────
        barManager1.Form = this;
        barManager1.Bars.AddRange(new Bar[] { bar1 });
        barManager1.DockControls.Add(barDockControlTop);
        barManager1.DockControls.Add(barDockControlBottom);
        barManager1.DockControls.Add(barDockControlLeft);
        barManager1.DockControls.Add(barDockControlRight);
        barManager1.Items.AddRange(new BarItem[] {
            btnNeu, btnBearbeiten, btnLoeschen,
            btnBeantragen, btnGenehmigen, btnVorlErhebung, btnAktualisieren
        });
        barManager1.MaxItemId = 7;
        barManager1.AllowMoveBarOnToolbar = false;

        // bar1
        bar1.BarName   = "Hauptfunktionen";
        bar1.DockCol   = 0;
        bar1.DockRow   = 0;
        bar1.DockStyle = BarDockStyle.Top;
        bar1.LinksPersistInfo.AddRange(new LinkPersistInfo[] {
            new LinkPersistInfo(btnNeu),
            new LinkPersistInfo(btnBearbeiten),
            new LinkPersistInfo(btnLoeschen),
            new LinkPersistInfo(btnBeantragen, true),
            new LinkPersistInfo(btnGenehmigen),
            new LinkPersistInfo(btnVorlErhebung),
            new LinkPersistInfo(btnAktualisieren, true)
        });
        bar1.OptionsBar.AllowQuickCustomization = false;
        bar1.OptionsBar.DrawDragBorder         = false;
        bar1.OptionsBar.UseWholeRow            = true;
        bar1.Text = "Hauptfunktionen";

        // Buttons
        ConfigBarButton(btnNeu,           "Neu",            0, "Neuen A1-Antrag anlegen (Strg+N)");
        ConfigBarButton(btnBearbeiten,    "Bearbeiten",     1, "Markierten Datensatz bearbeiten (F2)");
        ConfigBarButton(btnLoeschen,      "Löschen",        2, "Markierten Datensatz löschen");
        ConfigBarButton(btnBeantragen,    "Beantragen",     3, "A1-Antrag als beantragt markieren");
        ConfigBarButton(btnGenehmigen,    "Genehmigen",     4, "A1-Antrag als genehmigt markieren");
        ConfigBarButton(btnVorlErhebung,  "Vorl. Erhebung", 5, "Vorläufige Erhebung setzen");
        ConfigBarButton(btnAktualisieren, "Aktualisieren (F5)", 6, "Daten neu laden");

        SetBarButtonColor(btnLoeschen,     Color.DarkRed);
        SetBarButtonColor(btnBeantragen,   Color.FromArgb(180, 100, 0));
        SetBarButtonColor(btnGenehmigen,   Color.DarkGreen);
        SetBarButtonColor(btnVorlErhebung, Color.DarkBlue);

        btnNeu.ItemClick           += (_, _) => btnNeu_Click(this, EventArgs.Empty);
        btnBearbeiten.ItemClick    += (_, _) => btnBearbeiten_Click(this, EventArgs.Empty);
        btnLoeschen.ItemClick      += (_, _) => btnLoeschen_Click(this, EventArgs.Empty);
        btnBeantragen.ItemClick    += (_, _) => btnBeantragen_Click(this, EventArgs.Empty);
        btnGenehmigen.ItemClick    += (_, _) => btnGenehmigen_Click(this, EventArgs.Empty);
        btnVorlErhebung.ItemClick  += (_, _) => btnVorlErhebung_Click(this, EventArgs.Empty);
        btnAktualisieren.ItemClick += (_, _) => btnAktualisieren_Click(this, EventArgs.Empty);

        // Dock controls
        barDockControlTop.CausesValidation = false;
        barDockControlTop.Dock    = DockStyle.Top;
        barDockControlTop.Manager = barManager1;

        barDockControlBottom.CausesValidation = false;
        barDockControlBottom.Dock    = DockStyle.Bottom;
        barDockControlBottom.Manager = barManager1;

        barDockControlLeft.CausesValidation = false;
        barDockControlLeft.Dock    = DockStyle.Left;
        barDockControlLeft.Manager = barManager1;

        barDockControlRight.CausesValidation = false;
        barDockControlRight.Dock    = DockStyle.Right;
        barDockControlRight.Manager = barManager1;

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

        gridView1.GridControl                       = gridControl1;
        gridView1.Name                              = "gridView1";
        gridView1.OptionsView.ShowGroupPanel        = true;
        gridView1.OptionsView.ColumnAutoWidth       = false;
        gridView1.OptionsView.EnableAppearanceEvenRow = true;
        gridView1.OptionsFind.AlwaysVisible         = true;
        gridView1.OptionsFind.FindMode              = DevExpress.XtraEditors.FindMode.Always;
        gridView1.OptionsBehavior.Editable          = false;
        gridView1.OptionsSelection.MultiSelect      = false;
        gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
        gridView1.FocusRectStyle                    = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
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

        Controls.Add(gridControl1);
        Controls.Add(grpFilter);
        Controls.Add(barDockControlLeft);
        Controls.Add(barDockControlRight);
        Controls.Add(barDockControlTop);
        Controls.Add(barDockControlBottom);
        Controls.Add(statusStrip1);

        Font       = new Font("Segoe UI", 9F);
        Name       = "Form1";
        Text       = "A1-Antrag Verwaltung – Servolift";
        KeyPreview = true;
        Load      += Form1_Load;
        KeyDown   += Form1_KeyDown;

        ((ISupportInitialize)barManager1).EndInit();
        ((ISupportInitialize)radioGroup1.Properties).EndInit();
        ((ISupportInitialize)grpFilter).EndInit();
        grpFilter.ResumeLayout(false);
        ((ISupportInitialize)gridView1).EndInit();
        ((ISupportInitialize)gridControl1).EndInit();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private static void ConfigBarButton(BarButtonItem item, string caption, int id, string hint)
    {
        item.Caption    = caption;
        item.Id         = id;
        item.Name       = "bar_" + id;
        item.PaintStyle = BarItemPaintStyle.Caption;
        item.Hint       = hint;
    }

    private static void SetBarButtonColor(BarButtonItem item, Color color)
    {
        item.ItemAppearance.Normal.ForeColor = color;
        item.ItemAppearance.Normal.Options.UseForeColor = true;
        item.ItemAppearance.Hovered.ForeColor = color;
        item.ItemAppearance.Hovered.Options.UseForeColor = true;
    }
}
