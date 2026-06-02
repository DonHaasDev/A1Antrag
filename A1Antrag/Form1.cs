using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace A1Antrag
{
    public partial class Form1 : Form
    {
        private readonly string _connectionString;
        private OracleConnection _connection;
        private DataTable _dataTable;

        public Form1(string connectionString)
        {
            _connectionString = connectionString;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _connection = new OracleConnection(_connectionString);
                _connection.Open();
                AppTracking.Track(_connection);
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
            try
            {
                Cursor = Cursors.WaitCursor;

                using var adapter = new OracleDataAdapter(
                    "SELECT LFDNR, PERS_NR, FAM_NAME, NAME_VORNAME, VON, BIS, " +
                    "KDNR, FIRMA, STRASSE, PLZ, ORT, LAND, ANSPRECH_NAME, ANSPRECH_VORNAME, " +
                    "STATUS, BEANTRAGT_JN, BEANTRAGT_AM, BEANTRAGT_VON, " +
                    "GENEHMIGT_JN, GENEHMIGT_AM, GENEHMIGT_VON, " +
                    "VORL_ERH_JN, VORL_ERH_AM, VORL_ERH_VON, " +
                    "ANGELEGT_AM, ANGELEGT_VON, BEARBEITET_AM, BEARBEITET_VON " +
                    "FROM SIVAS.SL_A1_ANTRAG_TAB ORDER BY LFDNR DESC",
                    _connection);

                _dataTable = new DataTable();
                adapter.Fill(_dataTable);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = _dataTable;

                SetupColumns();
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
            dataGridView1.Columns.Clear();

            AddCol("LFDNR", "Lfd.Nr.", 65, DataGridViewContentAlignment.MiddleRight);
            AddCol("PERS_NR", "Pers.Nr.", 65, DataGridViewContentAlignment.MiddleRight);
            AddCol("FAM_NAME", "Name", 110);
            AddCol("NAME_VORNAME", "Vorname", 110);
            AddCol("VON", "Von", 90, format: "dd.MM.yyyy");
            AddCol("BIS", "Bis", 90, format: "dd.MM.yyyy");
            AddCol("FIRMA", "Firma / Einsatzort", 200);
            AddCol("LAND", "Land", 80);
            AddCol("STATUS", "Status", 120);
            AddCol("BEANTRAGT_JN", "Beantr.", 60, DataGridViewContentAlignment.MiddleCenter);
            AddCol("GENEHMIGT_JN", "Genehmigt", 75, DataGridViewContentAlignment.MiddleCenter);
            AddCol("VORL_ERH_JN", "Vorl.Erh.", 70, DataGridViewContentAlignment.MiddleCenter);
            AddCol("ANGELEGT_VON", "Angelegt von", 110);
            AddCol("ANGELEGT_AM", "Angelegt am", 100, format: "dd.MM.yyyy");
        }

        private void AddCol(string field, string header, int width,
            DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleLeft,
            string format = null)
        {
            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = field,
                HeaderText = header,
                Width = width,
                DefaultCellStyle = { Alignment = align }
            };
            if (format != null)
                col.DefaultCellStyle.Format = format;
            dataGridView1.Columns.Add(col);
        }

        private void UpdateStatusBar()
        {
            int count = _dataTable?.Rows.Count ?? 0;
            statusLabel.Text = $"{count} Datensätze  |  Benutzer: {Environment.UserName}  |  Server: {_connection?.DataSource}";
        }

        private DataRow GetSelectedRow()
        {
            if (dataGridView1.CurrentRow == null) return null;
            int idx = dataGridView1.CurrentRow.Index;
            if (_dataTable == null || idx < 0 || idx >= _dataTable.Rows.Count) return null;
            return _dataTable.Rows[idx];
        }

        // --- Toolbar-Aktionen ---

        private void btnNeu_Click(object sender, EventArgs e)
        {
            using var form = new A1AntragDetailForm(_connection, null);
            if (form.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }

        private void btnBearbeiten_Click(object sender, EventArgs e)
        {
            var row = GetSelectedRow();
            if (row == null) { Hinweis(); return; }
            using var form = new A1AntragDetailForm(_connection, row);
            if (form.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }

        private void btnLoeschen_Click(object sender, EventArgs e)
        {
            var row = GetSelectedRow();
            if (row == null) { Hinweis(); return; }

            string name = $"{row["FAM_NAME"]}, {row["NAME_VORNAME"]}";
            if (MessageBox.Show($"Datensatz für '{name}' wirklich löschen?", "Löschen bestätigen",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            try
            {
                using var cmd = _connection.CreateCommand();
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

        private void btnBeantragen_Click(object sender, EventArgs e)
        {
            var row = GetSelectedRow();
            if (row == null) { Hinweis(); return; }

            if (row["BEANTRAGT_JN"].ToString() == "J" &&
                MessageBox.Show("Antrag ist bereits als beantragt markiert. Erneut setzen?",
                    "Hinweis", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            ExecuteWorkflow(row, "BEANTRAGT_JN", "BEANTRAGT_AM", "BEANTRAGT_VON", "Beantragt");
        }

        private void btnGenehmigen_Click(object sender, EventArgs e)
        {
            var row = GetSelectedRow();
            if (row == null) { Hinweis(); return; }
            ExecuteWorkflow(row, "GENEHMIGT_JN", "GENEHMIGT_AM", "GENEHMIGT_VON", "Genehmigt");
        }

        private void btnVorlErhebung_Click(object sender, EventArgs e)
        {
            var row = GetSelectedRow();
            if (row == null) { Hinweis(); return; }
            ExecuteWorkflow(row, "VORL_ERH_JN", "VORL_ERH_AM", "VORL_ERH_VON", "Vorl. Erhalten");
        }

        private void ExecuteWorkflow(DataRow row, string jnField, string amField, string vonField, string status)
        {
            try
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText =
                    $"UPDATE SIVAS.SL_A1_ANTRAG_TAB SET " +
                    $"{jnField} = 'J', {amField} = SYSDATE, {vonField} = :von, " +
                    $"STATUS = :status, BEARBEITET_AM = SYSDATE, BEARBEITET_VON = :bearbeitet_von " +
                    $"WHERE LFDNR = :lfdnr";
                cmd.Parameters.Add(new OracleParameter("von", Environment.UserName));
                cmd.Parameters.Add(new OracleParameter("status", status));
                cmd.Parameters.Add(new OracleParameter("bearbeitet_von", Environment.UserName));
                cmd.Parameters.Add(new OracleParameter("lfdnr", row["LFDNR"]));
                cmd.ExecuteNonQuery();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler:\n{ex.Message}", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAktualisieren_Click(object sender, EventArgs e) => LoadData();

        // --- Zeilenfarbe nach Status ---

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || _dataTable == null || e.RowIndex >= _dataTable.Rows.Count) return;
            string status = _dataTable.Rows[e.RowIndex]["STATUS"].ToString();

            e.CellStyle.BackColor = status switch
            {
                "Genehmigt"      => Color.FromArgb(198, 239, 206),
                "Beantragt"      => Color.FromArgb(255, 235, 156),
                "Vorl. Erhalten" => Color.FromArgb(199, 244, 255),
                _                => Color.White
            };
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e) => btnBearbeiten_Click(sender, e);

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N) { btnNeu_Click(sender, e); e.Handled = true; }
            else if (e.KeyCode == Keys.F2) { btnBearbeiten_Click(sender, e); e.Handled = true; }
            else if (e.KeyCode == Keys.F5) { LoadData(); e.Handled = true; }
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
}
