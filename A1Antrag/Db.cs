using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace A1Antrag;

internal static class Db
{
    // Holt einen SELECT-String aus einer Package-Funktion (kein Parameter).
    public static string PackageSql(OracleConnection conn, string functionName)
    {
        using var cmd = new OracleCommand(
            $"BEGIN :result := SIVAS.SL_A1_ANTRAG.{functionName}; END;", conn);
        cmd.Parameters.Add(new OracleParameter("result", OracleDbType.Varchar2, 4000)
            { Direction = ParameterDirection.Output });
        cmd.ExecuteNonQuery();
        return cmd.Parameters["result"].Value?.ToString() ?? "";
    }
}
