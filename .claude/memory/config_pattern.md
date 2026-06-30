---
name: config-pattern
description: Inline Config.cs statt appsettings.json – OracleProfile sealed record + static Config class
metadata:
  type: project
---

Zugangsdaten werden als `sealed record OracleProfile` + `static class Config` inline in `Config.cs` hinterlegt.
`appsettings.json` und `Microsoft.Extensions.Configuration` wurden vollständig entfernt.

**Why:** Einfacher als JSON-Config, kein DI-Overhead, Muster aus dem `oracle_exporter`-Projekt übernommen.

## Struktur
```csharp
// Config.cs – gitignored, enthält echtes Passwort
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
        DbPassword: "",          // ← echtes Passwort nur in lokaler Config.cs (gitignored)
        DbHost:     "10.10.10.36",
        DbPort:     1521,
        DbSid:      "LINUX"
    );
}
```

## Schutz-Mechanismus
- `**/Config.cs` in `.gitignore` → wird nie committet
- `Config.example.cs` ist im Repo (DbPassword: `""`)
- csproj excludes `Config.example.cs` von der Kompilierung:
  ```xml
  <Compile Remove="Config.example.cs" />
  <None Include="Config.example.cs" />
  ```

## Verwendung
```csharp
// Program.cs
Application.Run(new Form1(Config.Oracle.ConnectionString));
```
Connection wird als `string` übergeben, `OracleConnection` wird in `Form1` geöffnet und an `A1AntragDetailForm` weitergereicht.
