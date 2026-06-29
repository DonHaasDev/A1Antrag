// Vorlage: nach Config.cs kopieren und Zugangsdaten eintragen.
// Config.cs steht in .gitignore und wird nicht eingecheckt.
namespace A1Antrag;

internal sealed record OracleProfile(
    string DbUser, string DbPassword,
    string DbHost, int DbPort, string DbSid)
{
    public string ConnectionString =>
        $"User Id={DbUser};Password={DbPassword};" +
        $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={DbHost})(PORT={DbPort}))" +
        $"(CONNECT_DATA=(SID={DbSid})));";
}

internal static class Config
{
    public static readonly OracleProfile Oracle = new(
        DbUser:     "sivas",
        DbPassword: "",
        DbHost:     "10.10.10.36",
        DbPort:     1521,
        DbSid:      "LINUX"
    );
}
