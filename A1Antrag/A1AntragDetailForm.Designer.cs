#nullable enable
using System.ComponentModel;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace A1Antrag;

partial class A1AntragDetailForm
{
    private IContainer? components = null;

    // Left panel – shared
    private Panel pnlLeft = null!;
    private LabelControl lblMaHint = null!;
    private GridControl gcSelectedMa = null!;
    private GridView gvSelectedMa = null!;
    private SimpleButton btnClearMa = null!;

    private LabelControl lblVon = null!;
    private DateEdit dtpVon = null!;
    private LabelControl lblBis = null!;
    private DateEdit dtpBis = null!;

    private LabelControl lblKdnr = null!;
    private TextEdit txtKdnr = null!;
    private LabelControl lblFirma = null!;
    private TextEdit txtFirma = null!;
    private LabelControl lblStrasse = null!;
    private TextEdit txtStrasse = null!;
    private LabelControl lblPlz = null!;
    private TextEdit txtPlz = null!;
    private LabelControl lblOrt = null!;
    private TextEdit txtOrt = null!;
    private LabelControl lblLand = null!;
    private TextEdit txtLand = null!;
    private SimpleButton btnClearKunde = null!;

    private LabelControl lblInfoStatus = null!;
    private LabelControl lblPflichtHint = null!;
    private SimpleButton btnSpeichern = null!;
    private SimpleButton btnAbbrechen = null!;

    // Right panel – new mode only
    private Panel pnlRight = null!;
    private LabelControl lblMaTitle = null!;
    private GridControl gcMaSearch = null!;
    private GridView gvMaSearch = null!;
    private LabelControl lblGepaTitle = null!;
    private GridControl gcGepaSearch = null!;
    private GridView gvGepaSearch = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();
        SuspendLayout();

        const int lw  = 80;
        const int row = 28;
        const int lx  = 10;
        const int tx  = lx + lw + 6;       // 96
        const int tw  = 240;
        int       leftW = tw + tx + 10;    // ≈ 346

        // ── LEFT PANEL ──────────────────────────────────────────────
        pnlLeft = new Panel { Location = new Point(10, 10), Size = new Size(leftW, 610), AutoSize = false };

        lblMaHint = BuildInfoLabel("MA = Mitarbeiter  //  AP = Ansprechpartner",
            0, 0, leftW, 8F, FontStyle.Italic, Color.DimGray);

        gcSelectedMa = BuildGrid(0, 20, leftW - 36, 100, false, out gvSelectedMa);
        AddGridCol(gvSelectedMa, "FAM_NAME",     "MA_NACHNAME", 140);
        AddGridCol(gvSelectedMa, "NAME_VORNAME", "MA_VORNAME",  120);

        btnClearMa = BuildXButton(leftW - 30, 20, gcSelectedMa.Height);

        int y = gcSelectedMa.Bottom + 14;
        lblVon = BuildLabel("von", lx, y, lw);
        dtpVon = BuildDate(tx, y - 2, 130);
        y += row;
        lblBis = BuildLabel("bis", lx, y, lw);
        dtpBis = BuildDate(tx, y - 2, 130);

        y += row + 6;
        lblKdnr    = BuildLabel("KDNR",    lx, y, lw); txtKdnr    = BuildText(tx, y - 2, 80);  txtKdnr.Properties.MaxLength    = 10;
        y += row;
        lblFirma   = BuildLabel("Firma",   lx, y, lw); txtFirma   = BuildText(tx, y - 2, tw);  txtFirma.Properties.MaxLength   = 200;
        y += row;
        lblStrasse = BuildLabel("Strasse", lx, y, lw); txtStrasse = BuildText(tx, y - 2, tw);  txtStrasse.Properties.MaxLength = 200;
        y += row;
        lblPlz     = BuildLabel("PLZ",     lx, y, lw); txtPlz     = BuildText(tx, y - 2, 70);  txtPlz.Properties.MaxLength     = 12;
        lblOrt     = BuildLabel("Ort",     tx + 76, y, 30); txtOrt = BuildText(tx + 110, y - 2, tw - 110); txtOrt.Properties.MaxLength = 100;
        y += row;
        lblLand    = BuildLabel("Land",    lx, y, lw); txtLand    = BuildText(tx, y - 2, tw);  txtLand.Properties.MaxLength    = 30;
        txtLand.Text = "Deutschland";

        btnClearKunde = BuildXButton(leftW - 30, lblKdnr.Top - 2, lblLand.Bottom - lblKdnr.Top + 6);

        y = lblLand.Bottom + 16;
        lblInfoStatus = BuildInfoLabel("", lx, y, leftW - 6, 8.5F, FontStyle.Bold, Color.DarkRed);
        lblPflichtHint = BuildInfoLabel("Pflichtfelder", lx, y + 22, leftW - 6, 8F, FontStyle.Regular, Color.DimGray);

        int btnY = lblPflichtHint.Bottom + 12;
        btnSpeichern = new SimpleButton
        {
            Text     = "Speichern und beenden",
            Size     = new Size(170, 30),
            Location = new Point(lx, btnY)
        };
        btnSpeichern.Appearance.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSpeichern.Appearance.Options.UseFont = true;
        btnSpeichern.Click += btnSpeichern_Click;
        AcceptButton = btnSpeichern;

        btnAbbrechen = new SimpleButton
        {
            Text     = "Abbrechen",
            Size     = new Size(100, 30),
            Location = new Point(lx + 176, btnY)
        };
        btnAbbrechen.Click += btnAbbrechen_Click;
        CancelButton = btnAbbrechen;

        pnlLeft.Controls.AddRange(new Control[] {
            lblMaHint, gcSelectedMa, btnClearMa,
            lblVon, dtpVon, lblBis, dtpBis,
            lblKdnr, txtKdnr, lblFirma, txtFirma,
            lblStrasse, txtStrasse,
            lblPlz, txtPlz, lblOrt, txtOrt,
            lblLand, txtLand, btnClearKunde,
            lblInfoStatus, lblPflichtHint,
            btnSpeichern, btnAbbrechen
        });

        // ── RIGHT PANEL (new mode) ───────────────────────────────────
        const int rw = 530;
        pnlRight = new Panel
        {
            Location = new Point(pnlLeft.Right + 12, 10),
            Size     = new Size(rw, 610),
            Visible  = false
        };

        lblMaTitle = BuildInfoLabel("Mitarbeiter – Doppelklick übernimmt (Suche im Feld oben rechts im Grid)",
            0, 0, rw, 8.5F, FontStyle.Bold, Color.FromArgb(51, 102, 153));

        gcMaSearch = BuildGrid(0, lblMaTitle.Bottom + 4, rw, 200, true, out gvMaSearch);
        AddGridCol(gvMaSearch, "FAM_NAME",     "MA_NACHNAME", 200);
        AddGridCol(gvMaSearch, "NAME_VORNAME", "MA_VORNAME",  180);
        gvMaSearch.DoubleClick += MaGridView_DoubleClick;

        lblGepaTitle = new LabelControl
        {
            Text         = "Kunde (GEPA) – Doppelklick übernimmt die Felder links",
            Location     = new Point(0, gcMaSearch.Bottom + 12),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size         = new Size(rw, 22)
        };
        lblGepaTitle.Appearance.BackColor = Color.FromArgb(255, 235, 100);
        lblGepaTitle.Appearance.ForeColor = Color.Black;
        lblGepaTitle.Appearance.Font      = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblGepaTitle.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
        lblGepaTitle.Appearance.Options.UseBackColor   = true;
        lblGepaTitle.Appearance.Options.UseForeColor   = true;
        lblGepaTitle.Appearance.Options.UseFont        = true;
        lblGepaTitle.Appearance.Options.UseTextOptions = true;

        gcGepaSearch = BuildGrid(0, lblGepaTitle.Bottom + 4, rw, 300, true, out gvGepaSearch);
        AddGridCol(gvGepaSearch, "KDNR",    "KDNR",    62);
        AddGridCol(gvGepaSearch, "FIRMA",   "Firma",   155);
        AddGridCol(gvGepaSearch, "STRASSE", "Strasse", 110);
        AddGridCol(gvGepaSearch, "ORT",     "Ort",     85);
        AddGridCol(gvGepaSearch, "LAND",    "Land",    90);
        AddGridCol(gvGepaSearch, "PLZ",     "PLZ",     52);
        gvGepaSearch.DoubleClick += GepaGridView_DoubleClick;

        pnlRight.Controls.AddRange(new Control[] {
            lblMaTitle, gcMaSearch, lblGepaTitle, gcGepaSearch
        });

        // ── FORM ────────────────────────────────────────────────────
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        Font                = new Font("Segoe UI", 9F);
        FormBorderStyle     = FormBorderStyle.FixedDialog;
        MaximizeBox         = false;
        StartPosition       = FormStartPosition.CenterParent;
        Name                = "A1AntragDetailForm";
        Text                = "A1-Antrag";
        Load               += A1AntragDetailForm_Load;

        Controls.AddRange(new Control[] { pnlLeft, pnlRight });

        ResumeLayout(false);
        PerformLayout();
    }

    // ── Helpers ─────────────────────────────────────────────────────

    private GridControl BuildGrid(int x, int y, int w, int h, bool searchPanel, out GridView view)
    {
        var gc = new GridControl();
        view = new GridView();
        ((ISupportInitialize)gc).BeginInit();
        ((ISupportInitialize)view).BeginInit();

        gc.Location = new Point(x, y);
        gc.Size     = new Size(w, h);
        gc.MainView = view;
        gc.ViewCollection.AddRange(new BaseView[] { view });

        view.GridControl                       = gc;
        view.OptionsView.ShowGroupPanel        = false;
        view.OptionsView.ColumnAutoWidth       = false;
        view.OptionsBehavior.Editable          = false;
        view.OptionsSelection.MultiSelect      = false;
        view.OptionsSelection.EnableAppearanceFocusedCell = false;
        view.FocusRectStyle                    = DrawFocusRectStyle.RowFullFocus;
        if (searchPanel)
        {
            view.OptionsFind.AlwaysVisible = true;
            view.OptionsFind.FindMode      = DevExpress.XtraEditors.FindMode.Always;
        }

        ((ISupportInitialize)view).EndInit();
        ((ISupportInitialize)gc).EndInit();
        return gc;
    }

    private static void AddGridCol(GridView view, string field, string caption, int width)
    {
        var col = new GridColumn
        {
            FieldName    = field,
            Caption      = caption,
            Visible      = true,
            VisibleIndex = view.Columns.Count,
            Width        = width
        };
        col.OptionsColumn.AllowEdit = false;
        view.Columns.Add(col);
    }

    private static SimpleButton BuildXButton(int x, int y, int h)
    {
        var b = new SimpleButton
        {
            Text     = "X",
            Location = new Point(x, y),
            Size     = new Size(26, h)
        };
        b.Appearance.Font      = new Font("Segoe UI", 8F, FontStyle.Bold);
        b.Appearance.ForeColor = Color.DarkRed;
        b.Appearance.Options.UseFont      = true;
        b.Appearance.Options.UseForeColor = true;
        return b;
    }

    private static LabelControl BuildLabel(string text, int x, int y, int w)
    {
        var l = new LabelControl
        {
            Text         = text + ":",
            Location     = new Point(x, y + 3),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size         = new Size(w, 20)
        };
        l.Appearance.TextOptions.HAlignment = HorzAlignment.Far;
        l.Appearance.TextOptions.VAlignment = VertAlignment.Center;
        l.Appearance.Options.UseTextOptions = true;
        return l;
    }

    private static LabelControl BuildInfoLabel(string text, int x, int y, int w,
        float size, FontStyle style, Color color)
    {
        var l = new LabelControl
        {
            Text         = text,
            Location     = new Point(x, y),
            AutoSizeMode = LabelAutoSizeMode.None,
            Size         = new Size(w, 20)
        };
        l.Appearance.Font      = new Font("Segoe UI", size, style);
        l.Appearance.ForeColor = color;
        l.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
        l.Appearance.TextOptions.VAlignment = VertAlignment.Center;
        l.Appearance.Options.UseFont        = true;
        l.Appearance.Options.UseForeColor   = true;
        l.Appearance.Options.UseTextOptions = true;
        return l;
    }

    private static TextEdit BuildText(int x, int y, int w)
    {
        var t = new TextEdit();
        ((ISupportInitialize)t.Properties).BeginInit();
        t.Location = new Point(x, y);
        t.Size     = new Size(w, 23);
        ((ISupportInitialize)t.Properties).EndInit();
        return t;
    }

    private static DateEdit BuildDate(int x, int y, int w)
    {
        var d = new DateEdit();
        ((ISupportInitialize)d.Properties).BeginInit();
        ((ISupportInitialize)d.Properties.CalendarTimeProperties).BeginInit();
        d.Location = new Point(x, y);
        d.Size     = new Size(w, 23);
        d.EditValue = DateTime.Today;
        d.Properties.DisplayFormat.FormatType   = FormatType.DateTime;
        d.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
        d.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
        d.Properties.Mask.EditMask = "dd.MM.yyyy";
        d.Properties.CalendarView  = DevExpress.XtraEditors.Repository.CalendarView.TouchUI;
        ((ISupportInitialize)d.Properties.CalendarTimeProperties).EndInit();
        ((ISupportInitialize)d.Properties).EndInit();
        return d;
    }
}
