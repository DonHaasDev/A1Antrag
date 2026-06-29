using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace A1Antrag;

public partial class Form1 : Form
{
    private readonly string _connectionString;
    private OracleConnection? _connection;
    private DataTable? _dataTable;

    public Form1(string connectionString)
    {
        _connectionString = connectionString;
        InitializeComponent();
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        try
        {
            _connection = new OracleConnection(_connectionString);
            _connection.Open();
            AppTracking.Track(_connection);
            SetupColumns();
            LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Verbindungsfehler:\n{ex.Message}", "Fehler",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
    }

    private void LoadData()
    {
        if (_connection == null) return;
        try
        {
            Cursor = Cursors.WaitCursor;

            bool offen = radioGroup1.SelectedIndex == 0;
            string filter = offen
                ? "WHERE STATUS NOT IN ('85 Stornierung beantragt','90 Antrag ungültig (löschen)')"
                : "WHERE ANGELEGT_AM >= SYSDATE - 730";

            using var adapter = new OracleDataAdapter(
                "SELECT LFDNR, PERS_NR, FAM_NAME, NAME_VORNAME, VON, BIS, " +
                "KDNR, FIRMA, STRASSE, PLZ, ORT, LAND, ANSPRECH_NAME, ANSPRECH_VORNAME, " +
                "STATUS, BEANTRAGT_JN, BEANTRAGT_AM, BEANTRAGT_VON, " +
                "GENEHMIGT_JN, GENEHMIGT_AM, GENEHMIGT_VON, " +
                "VORL_ERH_JN, VORL_ERH_AM, VORL_ERH_VON, " +
                "ANGELEGT_AM, ANGELEGT_VON, BEARBEITET_AM, BEARBEITET_VON " +
                $"FROM SIVAS.SL_A1_ANTRAG_TAB {filter} ORDER BY LFDNR DESC",
                _connection);

            _dataTable = new DataTable();
            adapter.Fill(_dataTable);

            gridControl1.DataSource = _dataTable;
            UpdateStatusBar();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Laden der Daten:\n{ex.Message}", "Fehler",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void SetupColumns()
    {
        gridView1.Columns.Clear();

        AddCol("LFDNR",        "Lfd.Nr.",          65,  HorzAlignment.Far);
        AddCol("PERS_NR",      "Pers.Nr.",          65,  HorzAlignment.Far);
        AddCol("FAM_NAME",     "Name",              110);
        AddCol("NAME_VORNAME", "Vorname",           110);
        AddCol("VON",          "Von",               90,  format: "dd.MM.yyyy");
        AddCol("BIS",          "Bis",               90,  format: "dd.MM.yyyy");
        AddCol("FIRMA",        "Firma / Einsatzort",200);
        AddCol("LAND",         "Land",              80);
        AddCol("STATUS",       "Status",            120);
        AddCol("BEANTRAGT_JN", "Beantr.",           60,  HorzAlignment.Center);
        AddCol("GENEHMIGT_JN", "Genehmigt",         75,  HorzAlignment.Center);
        AddCol("VORL_ERH_JN",  "Vorl.Erh.",         70,  HorzAlignment.Center);
        AddCol("ANGELEGT_VON", "Angelegt von",      110);
        AddCol("ANGELEGT_AM",  "Angelegt am",       100, format: "dd.MM.yyyy");
    }

    private void AddCol(string field, string header, int width,
        HorzAlignment align = HorzAlignment.Near, string? format = null)
    {
        var col = new GridColumn
        {
            FieldName    = field,
            Caption      = header,
            Visible      = true,
            VisibleIndex = gridView1.Columns.Count,
            Width        = width
        };
        col.AppearanceCell.TextOptions.HAlignment   = align;
        col.AppearanceHeader.TextOptions.HAlignment = align;
        col.OptionsColumn.AllowEdit = false;
        if (format != null)
        {
            col.DisplayFormat.FormatType   = FormatType.DateTime;
            col.DisplayFormat.FormatString = format;
        }
        gridView1.Columns.Add(col);
    }

    private void UpdateStatusBar()
    {
        int count = _dataTable?.Rows.Count ?? 0;
        statusLabel.Text = $"{count} Datensätze  |  Benutzer: {Environment.UserName}  |  Server: {_connection?.DataSource}";
    }

    private DataRow? GetSelectedRow()
    {
        int handle = gridView1.FocusedRowHandle;
        if (handle < 0) return null;
        return gridView1.GetDataRow(handle);
    }

    private void btnNeu_Click(object? sender, EventArgs e)
    {
        using var form = new A1AntragDetailForm(_connection!, null);
        if (form.ShowDialog(this) == DialogResult.OK)
            LoadData();
    }

    private void btnBearbeiten_Click(object? sender, EventArgs e)
    {
        var row = GetSelectedRow();
        if (row == null) { Hinweis(); return; }
        using var form = new A1AntragDetailForm(_connection!, row);
        if (form.ShowDialog(this) == DialogResult.OK)
            LoadData();
    }

    private void btnLoeschen_Click(object? sender, EventArgs e)
    {
        var row = GetSelectedRow();
        if (row == null) { Hinweis(); return; }

        string name = $"{row["FAM_NAME"]}, {row["NAME_VORNAME"]}";
        if (MessageBox.Show($"Datensatz für '{name}' wirklich löschen?", "Löschen bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        try
        {
            using var cmd = _connection!.CreateCommand();
            cmd.CommandText = "DELETE FROM SIVAS.SL_A1_ANTRAG_TAB WHERE LFDNR = :lfdnr";
            cmd.Parameters.Add(new OracleParameter("lfdnr", row["LFDNR"]));
            cmd.ExecuteNonQuery();
            LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Löschen:\n{ex.Message}", "Fehler",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnBeantragen_Click(object? sender, EventArgs e)
    {
        var row = GetSelectedRow();
        if (row == null) { Hinweis(); return; }

        string status = row["STATUS"]?.ToString() ?? "";
        if (status.StartsWith("30") &&
            MessageBox.Show("Antrag wurde bereits zur Beantragung weitergeleitet. Erneut setzen?",
                "Hinweis", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        CallUpdateDelete(row, "30");
    }

    private void btnGenehmigen_Click(object? sender, EventArgs e)
    {
        var row = GetSelectedRow();
        if (row == null) { Hinweis(); return; }
        CallUpdateDelete(row, "50");
    }

    private void btnVorlErhebung_Click(object? sender, EventArgs e)
    {
        var row = GetSelectedRow();
        if (row == null) { Hinweis(); return; }
        CallUpdateDelete(row, "40");
    }

    // Ruft SL_A1_ANTRAG.UPDATE_DELETE auf, das Status setzt UND E-Mail-Benachrichtigung auslöst.
    // i_function: '30'=Beantragen, '40'=Vorl.Bescheinigung, '50'=Bescheinigung erhalten,
    //             '80'=geändert, '85'=Stornierung, '90'=ungültig
    private void CallUpdateDelete(DataRow row, string function)
    {
        try
        {
            using var cmd = _connection!.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SL_A1_ANTRAG.UPDATE_DELETE";
            cmd.BindByName  = true;

            cmd.Parameters.Add(new OracleParameter("i_lfdnr",    OracleDbType.Decimal)  { Value = row["LFDNR"] });
            cmd.Parameters.Add(new OracleParameter("i_function", OracleDbType.Varchar2) { Value = function });
            cmd.Parameters.Add(new OracleParameter("i_user",     OracleDbType.Varchar2) { Value = Environment.UserName.ToUpper() });

            cmd.ExecuteNonQuery();
            LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler:\n{ex.Message}", "Fehler",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnAktualisieren_Click(object? sender, EventArgs e) => LoadData();

    // Status-Strings kommen direkt aus SL_A1_ANTRAG.UPDATE_DELETE / A1_ANLEGEN
    private void gridView1_RowStyle(object? sender, RowStyleEventArgs e)
    {
        if (e.RowHandle < 0) return;
        string status = gridView1.GetRowCellValue(e.RowHandle, "STATUS")?.ToString() ?? "";

        Color? bg = null;
        if      (status.StartsWith("50")) bg = Color.FromArgb(198, 239, 206); // Bescheinigung erhalten → grün
        else if (status.StartsWith("40")) bg = Color.FromArgb(199, 244, 255); // Vorl. Bescheinigung    → blau
        else if (status.StartsWith("30")) bg = Color.FromArgb(255, 235, 156); // Zur Beantragung        → gelb
        else if (status.StartsWith("85")) bg = Color.FromArgb(255, 199, 206); // Stornierung beantragt  → rot
        else if (status.StartsWith("90")) bg = Color.FromArgb(220, 220, 220); // Ungültig               → grau

        if (bg != null)
        {
            e.Appearance.BackColor      = bg.Value;
            e.Appearance.BackColor2     = bg.Value;
            e.Appearance.Options.UseBackColor = true;
        }
    }

    private void gridView1_DoubleClick(object? sender, EventArgs e)
    {
        var hit = gridView1.CalcHitInfo(gridControl1.PointToClient(MousePosition));
        if (!hit.InRow && !hit.InRowCell) return;
        btnBearbeiten_Click(sender, e);
    }

    private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.N)    { btnNeu_Click(sender, e); e.Handled = true; }
        else if (e.KeyCode == Keys.F2)           { btnBearbeiten_Click(sender, e); e.Handled = true; }
        else if (e.KeyCode == Keys.F5)           { LoadData(); e.Handled = true; }
    }

    private static void Hinweis() =>
        MessageBox.Show("Bitte einen Datensatz auswählen.", "Hinweis",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _connection?.Close();
        _connection?.Dispose();
        base.OnFormClosed(e);
    }
}
