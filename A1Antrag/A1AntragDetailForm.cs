using Devart.Data.Oracle;
using System;
using System.Data;
using System.Windows.Forms;

namespace A1Antrag
{
    public partial class A1AntragDetailForm : Form
    {
        private readonly OracleConnection _connection;
        private readonly DataRow _existingRow;
        private readonly bool _isNew;

        public A1AntragDetailForm(OracleConnection connection, DataRow existingRow)
        {
            _connection  = connection;
            _existingRow = existingRow;
            _isNew       = existingRow == null;
            InitializeComponent();
        }

        private void A1AntragDetailForm_Load(object sender, EventArgs e)
        {
            Text = _isNew ? "Neuer A1-Antrag" : "A1-Antrag bearbeiten";

            if (!_isNew)
            {
                txtPersNr.Text          = _existingRow["PERS_NR"] == DBNull.Value ? "" : _existingRow["PERS_NR"].ToString();
                txtFamName.Text         = _existingRow["FAM_NAME"].ToString();
                txtVorname.Text         = _existingRow["NAME_VORNAME"].ToString();
                txtKdnr.Text            = _existingRow["KDNR"] == DBNull.Value ? "" : _existingRow["KDNR"].ToString();
                txtFirma.Text           = _existingRow["FIRMA"].ToString();
                txtAnsprechName.Text    = _existingRow["ANSPRECH_NAME"].ToString();
                txtAnsprechVorname.Text = _existingRow["ANSPRECH_VORNAME"].ToString();
                txtStrasse.Text         = _existingRow["STRASSE"].ToString();
                txtPlz.Text             = _existingRow["PLZ"].ToString();
                txtOrt.Text             = _existingRow["ORT"].ToString();
                txtLand.Text            = _existingRow["LAND"].ToString();

                if (_existingRow["VON"] != DBNull.Value)
                    dtpVon.Value = Convert.ToDateTime(_existingRow["VON"]);
                if (_existingRow["BIS"] != DBNull.Value)
                    dtpBis.Value = Convert.ToDateTime(_existingRow["BIS"]);
            }
        }

        private void btnSpeichern_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFamName.Text) || string.IsNullOrWhiteSpace(txtVorname.Text))
            {
                MessageBox.Show("Name und Vorname sind Pflichtfelder.", "Pflichtfelder",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFamName.Focus();
                return;
            }

            if (dtpBis.Value < dtpVon.Value)
            {
                MessageBox.Show("'Bis'-Datum muss nach dem 'Von'-Datum liegen.", "Datumsvalidierung",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpBis.Focus();
                return;
            }

            try
            {
                using var cmd = _connection.CreateCommand();

                if (_isNew)
                {
                    cmd.CommandText = @"
                        INSERT INTO SIVAS.SL_A1_ANTRAG_TAB
                            (LFDNR, PERS_NR, FAM_NAME, NAME_VORNAME, VON, BIS,
                             KDNR, FIRMA, ANSPRECH_NAME, ANSPRECH_VORNAME, STRASSE, PLZ, ORT, LAND,
                             STATUS, BEANTRAGT_JN, GENEHMIGT_JN, VORL_ERH_JN,
                             ANGELEGT_AM, ANGELEGT_VON)
                        VALUES
                            ((SELECT NVL(MAX(LFDNR), 0) + 1 FROM SIVAS.SL_A1_ANTRAG_TAB),
                             :pers_nr, :fam_name, :name_vorname, :von, :bis,
                             :kdnr, :firma, :ansprech_name, :ansprech_vorname, :strasse, :plz, :ort, :land,
                             'Angelegt', 'N', 'N', 'N',
                             SYSDATE, :angelegt_von)";
                }
                else
                {
                    cmd.CommandText = @"
                        UPDATE SIVAS.SL_A1_ANTRAG_TAB SET
                            PERS_NR = :pers_nr, FAM_NAME = :fam_name, NAME_VORNAME = :name_vorname,
                            VON = :von, BIS = :bis,
                            KDNR = :kdnr, FIRMA = :firma, ANSPRECH_NAME = :ansprech_name,
                            ANSPRECH_VORNAME = :ansprech_vorname, STRASSE = :strasse,
                            PLZ = :plz, ORT = :ort, LAND = :land,
                            BEARBEITET_AM = SYSDATE, BEARBEITET_VON = :angelegt_von
                        WHERE LFDNR = :lfdnr";
                    cmd.Parameters.Add(new OracleParameter("lfdnr", _existingRow["LFDNR"]));
                }

                cmd.Parameters.Add(new OracleParameter("pers_nr",
                    string.IsNullOrWhiteSpace(txtPersNr.Text) ? (object)DBNull.Value : decimal.Parse(txtPersNr.Text)));
                cmd.Parameters.Add(new OracleParameter("fam_name",          txtFamName.Text.Trim().ToUpper()));
                cmd.Parameters.Add(new OracleParameter("name_vorname",      txtVorname.Text.Trim()));
                cmd.Parameters.Add(new OracleParameter("von",               dtpVon.Value.Date));
                cmd.Parameters.Add(new OracleParameter("bis",               dtpBis.Value.Date));
                cmd.Parameters.Add(new OracleParameter("kdnr",
                    string.IsNullOrWhiteSpace(txtKdnr.Text) ? (object)DBNull.Value : decimal.Parse(txtKdnr.Text)));
                cmd.Parameters.Add(new OracleParameter("firma",             txtFirma.Text.Trim()));
                cmd.Parameters.Add(new OracleParameter("ansprech_name",     txtAnsprechName.Text.Trim()));
                cmd.Parameters.Add(new OracleParameter("ansprech_vorname",  txtAnsprechVorname.Text.Trim()));
                cmd.Parameters.Add(new OracleParameter("strasse",           txtStrasse.Text.Trim()));
                cmd.Parameters.Add(new OracleParameter("plz",               txtPlz.Text.Trim()));
                cmd.Parameters.Add(new OracleParameter("ort",               txtOrt.Text.Trim()));
                cmd.Parameters.Add(new OracleParameter("land",              txtLand.Text.Trim()));
                cmd.Parameters.Add(new OracleParameter("angelegt_von",      Environment.UserName));

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

        private void btnAbbrechen_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
