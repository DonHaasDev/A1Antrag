---
name: project-overview
description: "A1Antrag – Zweck, Tech-Stack, Repository, Projektstruktur"
metadata: 
  node_type: memory
  type: project
  originSessionId: a99d4b5b-b27b-48d8-abae-031cc492aac1
---

A1Antrag ist eine WinForms-Desktopanwendung zur Verwaltung von A1-Entsendebescheinigungen für Servolift GmbH.

**Why:** Mitarbeiter, die ins Ausland reisen, benötigen A1-Nachweise. Die App legt Anträge an, verwaltet Status-Workflows und speichert die Daten in einer Oracle-Datenbank über gespeicherte Prozeduren.

**How to apply:** Tech-Stack und Datenbankdetails sind Basis für alle Weiterentwicklungen.

## Tech-Stack
- Framework: .NET 10, `net10.0-windows`, WinForms (`UseWindowsForms`, `EnableWindowsTargeting`)
- UI-Library: **DevExpress 24.2.15** (Paket `DevExpress.Win`)
- Datenbank: **Oracle** via `Oracle.ManagedDataAccess.Core` v23.7.0
- Build: `dotnet build` auf macOS möglich (Kompilierung), Ausführung nur auf Windows
- Repo: `git@github.com:DonHaasDev/A1Antrag.git`, Branch `main`

## Datenbankdetails
- Host: `10.10.10.36`, Port `1521`, SID `LINUX`
- Schema: `SIVAS`
- Stored Procedures: `SL_A1_ANTRAG.A1_ANLEGEN`, `SL_A1_ANTRAG.A1_BEARBEITEN`, `SL_A1_ANTRAG.UPDATE_DELETE`
- Wichtig: `cmd.BindByName = true` erforderlich für Oracle-Parameter
- Datumsformat für Prozeduren: `"dd.MM.yy"` (2-stelliges Jahr)

## Dateien (wichtig)
- `A1Antrag/Config.cs` — Zugangsdaten, **gitignored**, enthält echtes Passwort
- `A1Antrag/Config.example.cs` — Vorlage ohne Passwort, ist im Repo
- `A1Antrag/Form1.cs` + `Form1.Designer.cs` — Hauptliste
- `A1Antrag/A1AntragDetailForm.cs` + `.Designer.cs` — Neu/Bearbeiten-Dialog
- `build_and_run.bat` — Windows-Build-Skript, prüft Config.cs-Existenz

## Letzter Commit
`f647526` – "Fix UI issues: date validation, layout, GEPA grid, Stornieren button" (2026-06-29)
