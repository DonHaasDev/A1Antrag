---
name: devexpress-config
description: DevExpress 24.2 NuGet-Feed, Lizenzierung, Paketstruktur für dieses Projekt
metadata:
  type: project
---

## NuGet-Feed
- Key: siehe nuget.config im Solution-Root (nicht ins Repo-Memory committen)
- Feed-URL: `https://nuget.devexpress.com/<key>/api/v3/index.json`
- Konfiguriert in: `nuget.config` im Solution-Root

## Paket
- Ein einziges Meta-Paket: `DevExpress.Win` Version `24.2.15`
- Zieht automatisch: XtraGrid, XtraEditors, XtraBars, XtraLayout, Navigation

## Lizenzierung (.NET Core / .NET 5+)
- `licenses.licx` ist ein .NET-Framework-Konzept — **NICHT** für .NET Core/5+
- In .NET Core läuft die Lizenz ausschließlich über den authentifizierten NuGet-Feed-Key
- Kein `EmbeddedResource`-Eintrag für licx in der csproj nötig (MSB4803-Fehler vermeiden)

## Control-Mapping (Alt → Neu)
| Alt (Standard WinForms) | Neu (DevExpress) |
|---|---|
| DataGridView | GridControl + GridView |
| TextBox | TextEdit |
| DateTimePicker | DateEdit |
| RadioButton (Paar) | RadioGroup |
| GroupBox | GroupControl |
| Button | SimpleButton |
| Label | LabelControl |
| ToolStrip | BarManager + Bar + BarButtonItem |
| StatusStrip | bleibt StatusStrip |

## Wichtige Namespaces
- `DevExpress.XtraGrid` — GridControl, GridColumn
- `DevExpress.XtraGrid.Views.Grid` — GridView, DrawFocusRectStyle
- `DevExpress.XtraGrid.Views.Base` — BaseView
- `DevExpress.XtraEditors` — TextEdit, DateEdit, SimpleButton, RadioGroup, LabelControl, **FindMode** (!)
- `DevExpress.XtraBars` — BarManager, Bar, BarButtonItem, BarDockControl
- `DevExpress.Utils` — HorzAlignment, FormatType

**Why:** `FindMode` liegt in `DevExpress.XtraEditors`, NICHT in `DevExpress.XtraGrid.Views.Base` — häufige Verwechslung, führt zu CS0234.
