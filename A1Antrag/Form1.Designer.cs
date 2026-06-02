#nullable enable
using System.ComponentModel;

namespace A1Antrag;

partial class Form1
{
    private IContainer? components = null;

    private ToolStrip toolStrip1 = null!;
    private ToolStripButton btnNeu = null!;
    private ToolStripButton btnBearbeiten = null!;
    private ToolStripButton btnLoeschen = null!;
    private ToolStripSeparator sep1 = null!;
    private ToolStripButton btnBeantragen = null!;
    private ToolStripButton btnGenehmigen = null!;
    private ToolStripButton btnVorlErhebung = null!;
    private ToolStripSeparator sep2 = null!;
    private ToolStripButton btnAktualisieren = null!;
    private DataGridView dataGridView1 = null!;
    private StatusStrip statusStrip1 = null!;
    private ToolStripStatusLabel statusLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();

        toolStrip1       = new ToolStrip();
        btnNeu           = new ToolStripButton();
        btnBearbeiten    = new ToolStripButton();
        btnLoeschen      = new ToolStripButton();
        sep1             = new ToolStripSeparator();
        btnBeantragen    = new ToolStripButton();
        btnGenehmigen    = new ToolStripButton();
        btnVorlErhebung  = new ToolStripButton();
        sep2             = new ToolStripSeparator();
        btnAktualisieren = new ToolStripButton();
        dataGridView1    = new DataGridView();
        statusStrip1     = new StatusStrip();
        statusLabel      = new ToolStripStatusLabel();

        toolStrip1.SuspendLayout();
        ((ISupportInitialize)dataGridView1).BeginInit();
        statusStrip1.SuspendLayout();
        SuspendLayout();

        // ToolStrip
        toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
        toolStrip1.Padding   = new Padding(4, 2, 4, 2);
        toolStrip1.Items.AddRange(new ToolStripItem[] {
            btnNeu, btnBearbeiten, btnLoeschen, sep1,
            btnBeantragen, btnGenehmigen, btnVorlErhebung, sep2,
            btnAktualisieren
        });

        btnNeu.Text        = "  Neu  ";
        btnNeu.Font        = new Font("Segoe UI", 9F);
        btnNeu.ToolTipText = "Neuen A1-Antrag anlegen (Strg+N)";
        btnNeu.Click      += btnNeu_Click;

        btnBearbeiten.Text        = "  Bearbeiten  ";
        btnBearbeiten.Font        = new Font("Segoe UI", 9F);
        btnBearbeiten.ToolTipText = "Markierten Datensatz bearbeiten (F2)";
        btnBearbeiten.Click      += btnBearbeiten_Click;

        btnLoeschen.Text        = "  Löschen  ";
        btnLoeschen.Font        = new Font("Segoe UI", 9F);
        btnLoeschen.ForeColor   = Color.DarkRed;
        btnLoeschen.ToolTipText = "Markierten Datensatz löschen";
        btnLoeschen.Click      += btnLoeschen_Click;

        btnBeantragen.Text        = "  Beantragen  ";
        btnBeantragen.Font        = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnBeantragen.ForeColor   = Color.FromArgb(180, 100, 0);
        btnBeantragen.ToolTipText = "A1-Antrag als beantragt markieren";
        btnBeantragen.Click      += btnBeantragen_Click;

        btnGenehmigen.Text        = "  Genehmigen  ";
        btnGenehmigen.Font        = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnGenehmigen.ForeColor   = Color.DarkGreen;
        btnGenehmigen.ToolTipText = "A1-Antrag als genehmigt markieren";
        btnGenehmigen.Click      += btnGenehmigen_Click;

        btnVorlErhebung.Text        = "  Vorl. Erhebung  ";
        btnVorlErhebung.Font        = new Font("Segoe UI", 9F);
        btnVorlErhebung.ForeColor   = Color.DarkBlue;
        btnVorlErhebung.ToolTipText = "Vorläufige Erhebung setzen";
        btnVorlErhebung.Click      += btnVorlErhebung_Click;

        btnAktualisieren.Text        = "  Aktualisieren (F5)  ";
        btnAktualisieren.Font        = new Font("Segoe UI", 9F);
        btnAktualisieren.ToolTipText = "Daten neu laden";
        btnAktualisieren.Click      += btnAktualisieren_Click;

        // DataGridView
        dataGridView1.Dock                  = DockStyle.Fill;
        dataGridView1.AllowUserToAddRows    = false;
        dataGridView1.AllowUserToDeleteRows = false;
        dataGridView1.ReadOnly              = true;
        dataGridView1.SelectionMode         = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.MultiSelect           = false;
        dataGridView1.AutoGenerateColumns   = false;
        dataGridView1.RowHeadersWidth       = 4;
        dataGridView1.RowHeadersWidthSizeMode     = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        dataGridView1.AutoSizeRowsMode            = DataGridViewAutoSizeRowsMode.None;
        dataGridView1.RowTemplate.Height          = 22;
        dataGridView1.BorderStyle                 = BorderStyle.None;
        dataGridView1.BackgroundColor             = SystemColors.Window;
        dataGridView1.GridColor                   = Color.FromArgb(220, 220, 220);
        dataGridView1.CellBorderStyle             = DataGridViewCellBorderStyle.SingleHorizontal;
        dataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(51, 102, 153),
            ForeColor = Color.White,
            Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            Padding   = new Padding(4, 0, 0, 0)
        };
        dataGridView1.EnableHeadersVisualStyles   = false;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dataGridView1.ColumnHeadersHeight         = 26;
        dataGridView1.DefaultCellStyle.Font       = new Font("Segoe UI", 9F);
        dataGridView1.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(245, 248, 252)
        };
        dataGridView1.CellFormatting += dataGridView1_CellFormatting;
        dataGridView1.DoubleClick    += dataGridView1_DoubleClick;

        // StatusStrip
        statusStrip1.Items.AddRange(new ToolStripItem[] { statusLabel });
        statusStrip1.BackColor = Color.FromArgb(240, 240, 240);
        statusLabel.Text      = "Verbinde …";
        statusLabel.Spring    = true;
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;

        // Form
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(1280, 720);
        MinimumSize         = new Size(900, 500);
        Controls.Add(dataGridView1);
        Controls.Add(toolStrip1);
        Controls.Add(statusStrip1);
        Font       = new Font("Segoe UI", 9F);
        Name       = "Form1";
        Text       = "A1-Antrag Verwaltung – Servolift";
        KeyPreview = true;
        Load      += Form1_Load;
        KeyDown   += Form1_KeyDown;

        toolStrip1.ResumeLayout(false);
        toolStrip1.PerformLayout();
        ((ISupportInitialize)dataGridView1).EndInit();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
