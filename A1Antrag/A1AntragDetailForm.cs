using DevExpress.XtraEditors;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace A1Antrag;

public partial class A1AntragDetailForm : Form
{
    private readonly OracleConnection _connection;
    private readonly DataRow? _existingRow;
    private readonly bool _isNew;
    private readonly DataTable _selectedMa = new();

    public A1AntragDetailForm(OracleConnection connection, DataRow? existingRow)
    {
        _connection  = connection;
        _existingRow = existingRow;
        _isNew       = existingRow == null;
        InitializeComponent();
    }

    private void A1AntragDetailForm_Load(object? sender, EventArgs e)
    {
        _selectedMa.Columns.Add("PERS_NR",      typeof(decimal));
        _selectedMa.Columns.Add("FAM_NAME",     typeof(string));
        _selectedMa.Columns.Add("NAME_VORNAME", typeof(string));
        gcSelectedMa.DataSource = _selectedMa;

        btnClearMa.Click    += (_, _) => _selectedMa.Clear();
        btnClearKunde.Click += BtnClearKunde_Click;

        if (_isNew)
            SetupNewMode();
        else
            SetupEditMode();

        // Adjust form height to content
        ClientSize  = new Size(ClientSize.Width, pnlLeft.Top + btnSpeichern.Bottom + 26);
        MinimumSize = new Size(Width, Height);
        MaximumSize = new Size(Width, Height);
    }

    // ── NEW MODE ────────────────────────────────────────────────────

    private void SetupNewMode()
    {
        Text              = "Neuer A1-Antrag";
        btnSpeichern.Text = "Speichern und beenden";

        pnlRight.Visible = true;
        ClientSize = new Size(pnlRight.Right + 16, ClientSize.Height);

        lblInfoStatus.Text = "Felder werden automatisch befüllt";
        SetLabelColor(lblInfoStatus, Color.DimGray);
        lblPflichtHint.Text = "Pflichtfelder: Firma, Land, mind. 1 Mitarbeiter";

        txtLand.Text = "Deutschland";
        MarkRequired(txtFirma);
        MarkRequired(txtLand);

        gcMaSearch.DataSource   = LoadAllMa();
        gcGepaSearch.DataSource = LoadAllGepa();
    }

    private DataTable LoadAllMa()
    {
        var dt = new DataTable();
        try
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText =
                "SELECT t.persnr, t.familienname AS FAM_NAME, t.vorname AS NAME_VORNAME " +
                "FROM SIVAS.PDE_PERSSTAMM t " +
                "WHERE NVL(ausblenden_jn,'N') = 'N' " +
                "  AND (TO_CHAR(austrdat,'yyyy') >= TO_CHAR(SYSDATE,'yyyy') OR austrdat IS NULL) " +
                "  AND (TO_CHAR(eintrdat,'YYYY') <= TO_CHAR(SYSDATE,'yyyy')) " +
                "ORDER BY t.familienname";
            using var adapter = new OracleDataAdapter(cmd);
            adapter.Fill(dt);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Laden der Mitarbeiter:\n{ex.Message}", "Fehler",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return dt;
    }

    private DataTable LoadAllGepa()
    {
        var dt = new DataTable();
        try
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText =
                "SELECT t.kdnr, NVL(t.name1,t.name2) AS FIRMA, t.strasse, t.ort, t.land, t.plz " +
                "FROM SIVAS.GEPA t " +
                "WHERE t.gepa_c1 IN ('K','L','I') " +
                "  AND t.land NOT IN ('Deutschland') " +
                "  AND t.kz_aktiv = 'J' " +
                "ORDER BY NVL(t.name1,t.name2)";
            using var adapter = new OracleDataAdapter(cmd);
            adapter.Fill(dt);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Laden der Kunden:\n{ex.Message}", "Fehler",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return dt;
    }

    private void MaGridView_DoubleClick(object? sender, EventArgs e)
    {
        var src = gvMaSearch.GetFocusedDataRow();
        if (src == null) return;

        decimal persNr = src["PERSNR"] == DBNull.Value ? 0 : Convert.ToDecimal(src["PERSNR"]);
        string famName = src["FAM_NAME"].ToString() ?? "";
        string vorname = src["NAME_VORNAME"].ToString() ?? "";

        bool exists = _selectedMa.AsEnumerable()
            .Any(r => Convert.ToDecimal(r["PERS_NR"]) == persNr);
        if (!exists)
            _selectedMa.Rows.Add(persNr, famName, vorname);
    }

    private void GepaGridView_DoubleClick(object? sender, EventArgs e)
    {
        var row = gvGepaSearch.GetFocusedDataRow();
        if (row == null) return;

        txtKdnr.Text    = row["KDNR"]    == DBNull.Value ? "" : row["KDNR"].ToString()!;
        txtFirma.Text   = row["FIRMA"]   == DBNull.Value ? "" : row["FIRMA"].ToString()!;
        txtStrasse.Text = row["STRASSE"] == DBNull.Value ? "" : row["STRASSE"].ToString()!;
        txtPlz.Text     = row["PLZ"]     == DBNull.Value ? "" : row["PLZ"].ToString()!;
        txtOrt.Text     = row["ORT"]     == DBNull.Value ? "" : row["ORT"].ToString()!;
        txtLand.Text    = row["LAND"]    == DBNull.Value ? "" : row["LAND"].ToString()!;

        UnmarkRequired(txtFirma);
        UnmarkRequired(txtLand);
    }

    private void BtnClearKunde_Click(object? sender, EventArgs e)
    {
        txtKdnr.Text    = "";
        txtFirma.Text   = "";
        txtStrasse.Text = "";
        txtPlz.Text     = "";
        txtOrt.Text     = "";
        txtLand.Text    = "Deutschland";
        MarkRequired(txtFirma);
    }

    // ── EDIT MODE ───────────────────────────────────────────────────

    private void SetupEditMode()
    {
        Text              = "A1-Antrag bearbeiten";
        btnSpeichern.Text = "Änderungen Speichern und beenden";
        pnlRight.Visible  = false;
        ClientSize = new Size(pnlLeft.Right + 16, ClientSize.Height);

        _selectedMa.Rows.Add(
            _existingRow!["PERS_NR"] == DBNull.Value ? DBNull.Value : _existingRow["PERS_NR"],
            _existingRow["FAM_NAME"].ToString(),
            _existingRow["NAME_VORNAME"].ToString());

        btnClearMa.Enabled    = false;
        btnClearKunde.Enabled = false;

        if (_existingRow["VON"] != DBNull.Value) dtpVon.EditValue = Convert.ToDateTime(_existingRow["VON"]);
        if (_existingRow["BIS"] != DBNull.Value) dtpBis.EditValue = Convert.ToDateTime(_existingRow["BIS"]);

        txtKdnr.Text    = _existingRow["KDNR"]    == DBNull.Value ? "" : _existingRow["KDNR"].ToString()!;
        txtFirma.Text   = _existingRow["FIRMA"]?.ToString()   ?? "";
        txtStrasse.Text = _existingRow["STRASSE"]?.ToString() ?? "";
        txtPlz.Text     = _existingRow["PLZ"]?.ToString()     ?? "";
        txtOrt.Text     = _existingRow["ORT"]?.ToString()     ?? "";
        txtLand.Text    = _existingRow["LAND"]?.ToString()    ?? "";

        LockField(txtKdnr);
        LockField(txtFirma, Color.FromArgb(255, 199, 206));
        LockField(txtLand,  Color.FromArgb(255, 199, 206));

        lblInfoStatus.Text = "Felder können nicht mehr geändert werden";
        SetLabelColor(lblInfoStatus, Color.DarkRed);
        lblPflichtHint.Visible = false;
    }

    // ── SAVE ────────────────────────────────────────────────────────

    private void btnSpeichern_Click(object? sender, EventArgs e)
    {
        if (_isNew) SaveNew();
        else        SaveEdit();
    }

    private void SaveNew()
    {
        if (_selectedMa.Rows.Count == 0)
        {
            MessageBox.Show("Bitte mindestens einen Mitarbeiter auswählen.", "Pflichtfeld",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(txtFirma.Text))
        {
            MarkRequired(txtFirma);
            MessageBox.Show("Firma ist ein Pflichtfeld.", "Pflichtfeld",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtFirma.Focus();
            return;
        }
        if (string.IsNullOrWhiteSpace(txtLand.Text))
        {
            MarkRequired(txtLand);
            MessageBox.Show("Land ist ein Pflichtfeld.", "Pflichtfeld",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtLand.Focus();
            return;
        }
        if (dtpBis.DateTime.Date < dtpVon.DateTime.Date)
        {
            MessageBox.Show("'Bis'-Datum muss nach dem 'Von'-Datum liegen.", "Datumsvalidierung",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            foreach (DataRow maRow in _selectedMa.Rows)
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SL_A1_ANTRAG.A1_ANLEGEN";
                cmd.BindByName  = true;

                cmd.Parameters.Add(new OracleParameter("i_pers_nr",      OracleDbType.Decimal)  { Value = maRow["PERS_NR"] });
                cmd.Parameters.Add(new OracleParameter("i_fam_name",      OracleDbType.Varchar2) { Value = maRow["FAM_NAME"].ToString()?.ToUpper() ?? "" });
                cmd.Parameters.Add(new OracleParameter("i_name_vorname",  OracleDbType.Varchar2) { Value = maRow["NAME_VORNAME"].ToString() ?? "" });
                cmd.Parameters.Add(new OracleParameter("i_von",           OracleDbType.Varchar2) { Value = dtpVon.DateTime.ToString("dd.MM.yy") });
                cmd.Parameters.Add(new OracleParameter("i_bis",           OracleDbType.Varchar2) { Value = dtpBis.DateTime.ToString("dd.MM.yy") });

                string kdnrText = txtKdnr.Text.Trim();
                var kdnrParam = new OracleParameter("i_kdnr", OracleDbType.Decimal);
                if (string.IsNullOrEmpty(kdnrText))
                    kdnrParam.Value = DBNull.Value;
                else
                    kdnrParam.Value = decimal.Parse(kdnrText);
                cmd.Parameters.Add(kdnrParam);

                cmd.Parameters.Add(new OracleParameter("i_firma",        OracleDbType.Varchar2) { Value = txtFirma.Text.Trim() });
                cmd.Parameters.Add(new OracleParameter("i_strasse",      OracleDbType.Varchar2) { Value = txtStrasse.Text.Trim() });
                cmd.Parameters.Add(new OracleParameter("i_plz",          OracleDbType.Varchar2) { Value = txtPlz.Text.Trim() });
                cmd.Parameters.Add(new OracleParameter("i_ort",          OracleDbType.Varchar2) { Value = txtOrt.Text.Trim() });
                cmd.Parameters.Add(new OracleParameter("i_land",         OracleDbType.Varchar2) { Value = txtLand.Text.Trim() });
                cmd.Parameters.Add(new OracleParameter("i_angelegt_von", OracleDbType.Varchar2) { Value = Environment.UserName.ToUpper() });

                cmd.ExecuteNonQuery();
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Speichern:\n{ex.Message}", "Fehler",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SaveEdit()
    {
        if (dtpBis.DateTime.Date < dtpVon.DateTime.Date)
        {
            MessageBox.Show("'Bis'-Datum muss nach dem 'Von'-Datum liegen.", "Datumsvalidierung",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            string currentStatus = _existingRow!["STATUS"]?.ToString() ?? "";

            using var cmd = _connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SL_A1_ANTRAG.A1_BEARBEITEN";
            cmd.BindByName  = true;

            cmd.Parameters.Add(new OracleParameter("i_lfdnr",     OracleDbType.Decimal)  { Value = _existingRow["LFDNR"] });
            cmd.Parameters.Add(new OracleParameter("i_von",       OracleDbType.Varchar2) { Value = dtpVon.DateTime.ToString("dd.MM.yy") });
            cmd.Parameters.Add(new OracleParameter("i_bis",       OracleDbType.Varchar2) { Value = dtpBis.DateTime.ToString("dd.MM.yy") });
            cmd.Parameters.Add(new OracleParameter("i_strasse",   OracleDbType.Varchar2) { Value = txtStrasse.Text.Trim() });
            cmd.Parameters.Add(new OracleParameter("i_plz",       OracleDbType.Varchar2) { Value = txtPlz.Text.Trim() });
            cmd.Parameters.Add(new OracleParameter("i_ort",       OracleDbType.Varchar2) { Value = txtOrt.Text.Trim() });
            cmd.Parameters.Add(new OracleParameter("i_user",      OracleDbType.Varchar2) { Value = Environment.UserName.ToUpper() });
            cmd.Parameters.Add(new OracleParameter("i_newstatus", OracleDbType.Varchar2) { Value = currentStatus });

            cmd.ExecuteNonQuery();

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Speichern:\n{ex.Message}", "Fehler",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnAbbrechen_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    // ── Helpers ─────────────────────────────────────────────────────

    private static void LockField(TextEdit txt, Color? bg = null)
    {
        Color c = bg ?? SystemColors.Control;
        txt.Properties.ReadOnly = true;
        txt.Properties.Appearance.BackColor        = c;
        txt.Properties.AppearanceReadOnly.BackColor = c;
        txt.Properties.Appearance.Options.UseBackColor        = true;
        txt.Properties.AppearanceReadOnly.Options.UseBackColor = true;
    }

    private static void MarkRequired(TextEdit txt)
    {
        txt.Properties.Appearance.BackColor = Color.FromArgb(255, 199, 206);
        txt.Properties.Appearance.Options.UseBackColor = true;
    }

    private static void UnmarkRequired(TextEdit txt)
    {
        txt.Properties.Appearance.BackColor = SystemColors.Window;
        txt.Properties.Appearance.Options.UseBackColor = false;
    }

    private static void SetLabelColor(LabelControl lbl, Color color)
    {
        lbl.Appearance.ForeColor = color;
        lbl.Appearance.Options.UseForeColor = true;
    }
}
