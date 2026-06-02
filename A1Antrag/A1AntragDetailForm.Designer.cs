#nullable enable
using System.ComponentModel;

namespace A1Antrag;

partial class A1AntragDetailForm
{
    private IContainer? components = null;

    private Label lblPersNr = null!;
    private TextBox txtPersNr = null!;
    private Label lblFamName = null!;
    private TextBox txtFamName = null!;
    private Label lblVorname = null!;
    private TextBox txtVorname = null!;

    private Label lblVon = null!;
    private DateTimePicker dtpVon = null!;
    private Label lblBis = null!;
    private DateTimePicker dtpBis = null!;

    private Label lblKdnr = null!;
    private TextBox txtKdnr = null!;
    private Label lblFirma = null!;
    private TextBox txtFirma = null!;
    private Label lblAnsprechName = null!;
    private TextBox txtAnsprechName = null!;
    private Label lblAnsprechVorname = null!;
    private TextBox txtAnsprechVorname = null!;
    private Label lblStrasse = null!;
    private TextBox txtStrasse = null!;
    private Label lblPlz = null!;
    private TextBox txtPlz = null!;
    private Label lblOrt = null!;
    private TextBox txtOrt = null!;
    private Label lblLand = null!;
    private TextBox txtLand = null!;

    private Button btnSpeichern = null!;
    private Button btnAbbrechen = null!;

    private GroupBox grpMitarbeiter = null!;
    private GroupBox grpZeitraum = null!;
    private GroupBox grpEinsatzort = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();

        grpMitarbeiter     = new GroupBox();
        grpZeitraum        = new GroupBox();
        grpEinsatzort      = new GroupBox();

        lblPersNr          = new Label();
        txtPersNr          = new TextBox();
        lblFamName         = new Label();
        txtFamName         = new TextBox();
        lblVorname         = new Label();
        txtVorname         = new TextBox();

        lblVon             = new Label();
        dtpVon             = new DateTimePicker();
        lblBis             = new Label();
        dtpBis             = new DateTimePicker();

        lblKdnr            = new Label();
        txtKdnr            = new TextBox();
        lblFirma           = new Label();
        txtFirma           = new TextBox();
        lblAnsprechName    = new Label();
        txtAnsprechName    = new TextBox();
        lblAnsprechVorname = new Label();
        txtAnsprechVorname = new TextBox();
        lblStrasse         = new Label();
        txtStrasse         = new TextBox();
        lblPlz             = new Label();
        txtPlz             = new TextBox();
        lblOrt             = new Label();
        txtOrt             = new TextBox();
        lblLand            = new Label();
        txtLand            = new TextBox();

        btnSpeichern       = new Button();
        btnAbbrechen       = new Button();

        SuspendLayout();

        const int lw  = 130;
        const int tw  = 220;
        const int lh  = 20;
        const int th  = 23;
        const int row = 28;
        const int lx  = 12;
        int tx = lx + lw + 6;

        // === Mitarbeiter ===
        grpMitarbeiter.Text     = "Mitarbeiter";
        grpMitarbeiter.Font     = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpMitarbeiter.Location = new Point(12, 12);
        grpMitarbeiter.Size     = new Size(tx + tw + 12, 4 + row * 3 + 14);

        SetLabel(lblPersNr,  "Pers.Nr.:",  lx, 22,           lw, lh);
        SetText(txtPersNr,   tx, 20,           80, th);
        txtPersNr.MaxLength = 10;

        SetLabel(lblFamName, "Nachname:",  lx, 22 + row,     lw, lh);
        SetText(txtFamName,  tx, 20 + row,     tw, th);
        txtFamName.MaxLength = 20;
        txtFamName.CharacterCasing = CharacterCasing.Upper;

        SetLabel(lblVorname, "Vorname:",   lx, 22 + row * 2, lw, lh);
        SetText(txtVorname,  tx, 20 + row * 2, tw, th);
        txtVorname.MaxLength = 20;

        grpMitarbeiter.Controls.AddRange(new Control[] {
            lblPersNr, txtPersNr, lblFamName, txtFamName, lblVorname, txtVorname });

        // === Zeitraum ===
        int grp1Bottom = grpMitarbeiter.Bottom + 10;
        grpZeitraum.Text     = "Einsatzzeitraum";
        grpZeitraum.Font     = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpZeitraum.Location = new Point(12, grp1Bottom);
        grpZeitraum.Size     = new Size(tx + tw + 12, 4 + row * 2 + 14);

        SetLabel(lblVon, "Von:", lx, 22,       lw, lh);
        dtpVon.Location = new Point(tx, 20);
        dtpVon.Size     = new Size(140, th);
        dtpVon.Format   = DateTimePickerFormat.Short;
        dtpVon.Font     = new Font("Segoe UI", 9F);

        SetLabel(lblBis, "Bis:", lx, 22 + row, lw, lh);
        dtpBis.Location = new Point(tx, 20 + row);
        dtpBis.Size     = new Size(140, th);
        dtpBis.Format   = DateTimePickerFormat.Short;
        dtpBis.Font     = new Font("Segoe UI", 9F);

        grpZeitraum.Controls.AddRange(new Control[] { lblVon, dtpVon, lblBis, dtpBis });

        // === Einsatzort ===
        int grp2Bottom = grpZeitraum.Bottom + 10;
        grpEinsatzort.Text     = "Einsatzort / Auftraggeber";
        grpEinsatzort.Font     = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpEinsatzort.Location = new Point(12, grp2Bottom);
        grpEinsatzort.Size     = new Size(tx + tw + 12, 4 + row * 7 + 14);

        SetLabel(lblKdnr,            "Kd.Nr.:",        lx, 22,           lw, lh);
        SetText(txtKdnr,              tx, 20,           80, th);
        txtKdnr.MaxLength = 10;

        SetLabel(lblFirma,           "Firma:",          lx, 22 + row,     lw, lh);
        SetText(txtFirma,             tx, 20 + row,     tw, th);
        txtFirma.MaxLength = 200;

        SetLabel(lblAnsprechName,    "Ansp. Name:",     lx, 22 + row * 2, lw, lh);
        SetText(txtAnsprechName,      tx, 20 + row * 2, tw, th);
        txtAnsprechName.MaxLength = 200;

        SetLabel(lblAnsprechVorname, "Ansp. Vorname:",  lx, 22 + row * 3, lw, lh);
        SetText(txtAnsprechVorname,   tx, 20 + row * 3, tw, th);
        txtAnsprechVorname.MaxLength = 200;

        SetLabel(lblStrasse,         "Straße:",          lx, 22 + row * 4, lw, lh);
        SetText(txtStrasse,           tx, 20 + row * 4, tw, th);
        txtStrasse.MaxLength = 200;

        SetLabel(lblPlz, "PLZ:",    lx,        22 + row * 5, 40,      lh);
        SetText(txtPlz,   lx + 46,  20 + row * 5, 60,   th);
        txtPlz.MaxLength = 12;
        SetLabel(lblOrt, "Ort:",    lx + 116,  22 + row * 5, 30,      lh);
        SetText(txtOrt,   lx + 150, 20 + row * 5, tw - 30, th);
        txtOrt.MaxLength = 100;

        SetLabel(lblLand,            "Land:",            lx, 22 + row * 6, lw, lh);
        SetText(txtLand,              tx, 20 + row * 6, tw, th);
        txtLand.MaxLength = 30;
        txtLand.Text = "Deutschland";

        grpEinsatzort.Controls.AddRange(new Control[] {
            lblKdnr, txtKdnr, lblFirma, txtFirma,
            lblAnsprechName, txtAnsprechName, lblAnsprechVorname, txtAnsprechVorname,
            lblStrasse, txtStrasse, lblPlz, txtPlz, lblOrt, txtOrt, lblLand, txtLand });

        // === Buttons ===
        int btnTop = grpEinsatzort.Bottom + 16;

        btnSpeichern.Text      = "Speichern";
        btnSpeichern.Size      = new Size(100, 30);
        btnSpeichern.Location  = new Point(tx, btnTop);
        btnSpeichern.Font      = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSpeichern.BackColor = Color.FromArgb(51, 102, 153);
        btnSpeichern.ForeColor = Color.White;
        btnSpeichern.FlatStyle = FlatStyle.Flat;
        btnSpeichern.Click    += btnSpeichern_Click;
        AcceptButton           = btnSpeichern;

        btnAbbrechen.Text     = "Abbrechen";
        btnAbbrechen.Size     = new Size(100, 30);
        btnAbbrechen.Location = new Point(tx + 110, btnTop);
        btnAbbrechen.Font     = new Font("Segoe UI", 9F);
        btnAbbrechen.Click   += btnAbbrechen_Click;
        CancelButton          = btnAbbrechen;

        // === Form ===
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        int formW           = grpMitarbeiter.Right + 24;
        int formH           = btnTop + 30 + 50;
        ClientSize          = new Size(formW, formH);
        MinimumSize         = new Size(formW + 16, formH + 39);
        MaximizeBox         = false;
        FormBorderStyle     = FormBorderStyle.FixedDialog;
        StartPosition       = FormStartPosition.CenterParent;
        Font                = new Font("Segoe UI", 9F);
        Name                = "A1AntragDetailForm";
        Text                = "A1-Antrag";
        Load               += A1AntragDetailForm_Load;

        Controls.AddRange(new Control[] {
            grpMitarbeiter, grpZeitraum, grpEinsatzort, btnSpeichern, btnAbbrechen });

        ResumeLayout(false);
        PerformLayout();
    }

    private static void SetLabel(Label lbl, string text, int x, int y, int w, int h)
    {
        lbl.Text      = text;
        lbl.Location  = new Point(x, y + 2);
        lbl.Size      = new Size(w, h);
        lbl.Font      = new Font("Segoe UI", 9F);
        lbl.TextAlign = ContentAlignment.MiddleRight;
    }

    private static void SetText(TextBox txt, int x, int y, int w, int h)
    {
        txt.Location = new Point(x, y);
        txt.Size     = new Size(w, h);
        txt.Font     = new Font("Segoe UI", 9F);
    }
}
