---
name: known-pitfalls
description: "Bekannte Fehler und Fallstricke im A1Antrag-Projekt (DevExpress, Oracle, macOS-Build)"
metadata: 
  node_type: memory
  type: project
  originSessionId: a99d4b5b-b27b-48d8-abae-031cc492aac1
---

## CS0234: FindMode nicht in DevExpress.XtraGrid.Views.Base
`FindMode` liegt in **`DevExpress.XtraEditors`**, nicht in `DevExpress.XtraGrid.Views.Base`.
```csharp
// KORREKT:
view.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
// FALSCH (CS0234):
view.OptionsFind.FindMode = DevExpress.XtraGrid.Views.Base.FindMode.Always;
```

## MSB4803: LC-Task nicht unterstützt in .NET Core MSBuild
Ursache: `<EmbeddedResource Include="Properties\licenses.licx" />` im csproj.
Fix: `licenses.licx` komplett entfernen. DevExpress-Lizenz für .NET Core läuft über den authentifizierten NuGet-Feed-Key – kein licx nötig.

## PERS_NR vs. PERSNR (Spaltenname-Asymmetrie)
- MA-Such-Query gibt `PERSNR` zurück (Oracle-Spaltenname, kein Alias)
- `_selectedMa`-DataTable hat Spalte `PERS_NR` (mit Unterstrich)
- Lesen aus Suchergebnis: `src["PERSNR"]`
- Schreiben in Selected-Tabelle: `PERS_NR`
- Diese Asymmetrie ist bewusst beibehalten, nicht "vereinheitlichen"

## macOS Build: kein .exe
Auf macOS erzeugt `dotnet build` kein `A1Antrag.exe` – nur `A1Antrag` (ohne Extension).
Exit-Code `dotnet build` ist trotzdem 0 bei Erfolg – Prüfung auf `ls A1Antrag.exe` schlägt fehl, ist aber kein echter Build-Fehler.

## GridControl DataSource: am Container, nicht am View
```csharp
gridControl1.DataSource = dt;  // richtig
gridView1.DataSource = dt;     // falsch / hat keinen DataSource-Property
```

## DoubleClick auf GridView: HitTest nötig
```csharp
private void gridView1_DoubleClick(object? sender, EventArgs e)
{
    var hit = gridView1.CalcHitInfo(gridControl1.PointToClient(MousePosition));
    if (!hit.InRow && !hit.InRowCell) return;
    // ...
}
```
Ohne HitTest-Check reagiert DoubleClick auch auf Header-Klicks.

## DateEdit: EditValue vs. DateTime
- Setzen: `dtpVon.EditValue = Convert.ToDateTime(value)`
- Lesen: `dtpVon.DateTime` (gibt `DateTime` zurück)

## X-Button hinter Textfeldern (Layout-Bug)
`BuildXButton(leftW - 30, ...)` ist FALSCH wenn `tx + tw > leftW - 30`.
Mit alten Werten: `tx + tw = 336 > leftW - 30 = 316` → Button lag unter Textfeld, unsichtbar.
Fix: X-Button mit explizitem `xBtnX = tx + tw + 8` platzieren, `leftW = xBtnX + 28`.
Für `btnClearKunde` immer `BuildXButton(xBtnX, ...)` verwenden, nie `leftW - 30`.

## Formhöhe im Neu-Modus zu klein
Wenn `ClientSize.Height` nur aus `pnlLeft.Top + btnSpeichern.Bottom + 26` berechnet wird,
ist das rechte Panel (pnlRight) am unteren Rand abgeschnitten (Höhe ~495 vs. benötigte ~620px).
Fix: `Math.Max(..., pnlRight.Bottom + 10)` im Neu-Modus. Siehe [[architecture-decisions]].

## DateEdit Mask + DisableTextEditor: nicht kombinieren
`MaskType.DateTime` mit `EditMask` ist unnötig wenn `TextEditStyle = DisableTextEditor` gesetzt ist.
Mit DisableTextEditor ist Texteingabe sowieso gesperrt – die Mask hat keinen Effekt.
Nur `DisplayFormat` + `EditFormat` setzen, keine `Properties.Mask.*`-Einstellungen.

## CalendarView.TouchUI auf Desktop
DevExpress DateEdit mit `CalendarView.TouchUI` sieht auf Desktop-Windows unpassend aus.
Für Desktop-Apps: `CalendarView.Classic` verwenden.

## A1Antrag alt/-Ordner
Liegt lokal als ungetrackter Ordner mit Binary-Artefakten (bin/obj/.exe). Nicht committen.
