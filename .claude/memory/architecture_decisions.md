---
name: architecture-decisions
description: "Bewusst getroffene Designentscheidungen: GEPA-Laden, BarManager, SearchPanel, Datumsvalidierung, Layout-Konstanten, Toolbar-Sichtbarkeit"
metadata: 
  node_type: memory
  type: project
  originSessionId: a99d4b5b-b27b-48d8-abae-031cc492aac1
---

## GEPA-Ladestrategie: Vollständig laden, lokal filtern
Beim Öffnen des Neu-Anlage-Dialogs werden ALLE aktiven Auslandskunden einmalig geladen.
Das DevExpress SearchPanel filtert danach vollständig lokal – kein DB-Roundtrip pro Tastendruck.

**Why:** Benutzer hat explizit diese Option gewählt (vs. "Live-Suche ab 3 Zeichen"). Einfacher Code, schnellere UX bei typischen Datenmengen. Falls Datenmenge >10k Zeilen wird, kann auf serverseitige Suche umgestellt werden.

SQL:
```sql
SELECT t.kdnr, NVL(t.name1,t.name2) AS FIRMA, t.strasse, t.ort, t.land, t.plz
FROM SIVAS.GEPA t
WHERE t.gepa_c1 IN ('K','L','I')
  AND t.land NOT IN ('Deutschland')
  AND t.kz_aktiv = 'J'
ORDER BY NVL(t.name1,t.name2)
```

## Toolbar: BarManager statt ToolStrip
Alle Toolbar-Buttons laufen über `BarManager` + `Bar` + `BarButtonItem`.
Bestehende Click-Handler (`btnNeu_Click` etc.) wurden beibehalten – nur die Anbindung wechselte von `ToolStripButton.Click` auf `BarButtonItem.ItemClick`.
Tastenkürzel (Strg+N, F2, F5) bleiben über `Form1_KeyDown` erhalten.

**Why:** Benutzer hat explizit DevExpress-Toolbar gewählt für einheitlichen Skin-Look.

## SearchPanel statt TextBox-Suche
MA- und GEPA-Grids haben `OptionsFind.AlwaysVisible = true` und `FindMode = DevExpress.XtraEditors.FindMode.Always`.
Die alten `txtMaSearch`/`txtGepaSearch`-TextBoxen sowie `SearchMa()`/`SearchGepa()`-Methoden wurden vollständig entfernt.

**Why:** Weniger Code, bessere UX, DevExpress-Nativeness.

## Form-Größe: Dynamisch berechnet
`ClientSize` wird in `A1AntragDetailForm_Load` dynamisch anhand der Control-Positionen gesetzt.
Im Neu-Modus ist `pnlRight.Visible = true` → Formular breiter UND höher (Höhe = Max aus linkem und rechtem Panel).
Im Bearbeiten-Modus ist `pnlRight.Visible = false` → Formular schmal.

```csharp
int formHeight = pnlLeft.Top + btnSpeichern.Bottom + 26;
if (_isNew)
    formHeight = Math.Max(formHeight, pnlRight.Bottom + 10);
ClientSize = new Size(ClientSize.Width, formHeight);
```

**Why:** Ohne den Max-Vergleich wurde die Höhe nur aus dem linken Panel (~495px) berechnet, das rechte Panel (620px) war unten abgeschnitten.

## Toolbar-Buttons: Sichtbarkeit steuern
Nicht benötigte Buttons werden mit `BarItemVisibility.Never` ausgeblendet (nicht gelöscht – Click-Handler bleiben im Code).
Aktuell ausgeblendet: `btnLoeschen`, `btnBeantragen`, `btnGenehmigen`, `btnVorlErhebung`.
Sichtbar: `btnNeu`, `btnBearbeiten`, `btnStornieren`, `btnAktualisieren`.

`btnStornieren` ruft `CallUpdateDelete(row, "85")` auf → setzt Status "85 Stornierung beantragt".

## Datumsvalidierung Von/Bis
Event in `A1AntragDetailForm_Load` verdrahtet (gilt für Neu- UND Bearbeitungsmodus):
```csharp
dtpVon.DateTimeChanged += (_, _) =>
{
    if (dtpBis.DateTime.Date < dtpVon.DateTime.Date)
        dtpBis.EditValue = dtpVon.EditValue;
};
```
Zusätzlich prüfen `SaveNew()` und `SaveEdit()` nochmals beim Speichern.

## Left-Panel Layout-Konstanten
```csharp
const int lw    = 80;   // Label-Breite
const int lx    = 6;    // linker Rand
const int tx    = 92;   // Textfeld-Start (lx + lw + 6)
const int tw    = 210;  // Textfeld-Breite
const int xBtnX = 310;  // X-Button-Position (tx + tw + 8)
int       leftW = 338;  // Panel-Breite (xBtnX + 28)
```
X-Button liegt IMMER rechts der Textfelder. `btnClearKunde = BuildXButton(xBtnX, ...)` — nicht `leftW - 30`.

## GEPA-Grid: ColumnAutoWidth
```csharp
gvGepaSearch.OptionsView.ColumnAutoWidth = true;
```
Füllt alle Spalten automatisch auf Grid-Breite (640px). Kein Horizontal-Scroll mehr.

## GridControl DataSource: am Container setzen
```csharp
gridControl1.DataSource = _dataTable;  // NICHT gridView1.DataSource
```

## Status-Farben via RowStyle-Event
```csharp
gridView1.RowStyle += gridView1_RowStyle;
// Status-Präfixe: "50"=grün, "40"=blau, "30"=gelb, "85"=rot, "90"=grau
// Beide BackColor + BackColor2 setzen + Options.UseBackColor = true
```
