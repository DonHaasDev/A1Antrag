# A1Antrag – Verwaltung von A1-Entsendebescheinigungen

WinForms-Anwendung zur Verwaltung von A1-Anträgen (Entsendebescheinigungen nach Art. 19 VO (EG) Nr. 883/2004) für Servolift-Mitarbeiter, die vorübergehend im EU-Ausland tätig sind.

---

## Fachlicher Hintergrund

Der **A1-Antrag** (früher E101) ist eine Bescheinigung der Deutschen Rentenversicherung (DRV) oder der zuständigen Krankenkasse (GKV), die bestätigt, dass ein ins Ausland entsandter Mitarbeiter weiterhin dem deutschen Sozialversicherungsrecht unterliegt. Die Bescheinigung muss bei Entsendungen in EU/EWR-Staaten und die Schweiz beantragt werden und ist beim Arbeitseinsatz mitzuführen.

### Workflow

```
Anlegen → Beantragen (online bei DRV/GKV) → Genehmigt (Bescheinigung erhalten)
                    ↘ Vorl. Erhalten (vorläufige Bescheinigung)
```

| Status | Bedeutung |
|---|---|
| `Angelegt` | Antrag erfasst, noch nicht beantragt |
| `Beantragt` | Online-Antrag bei DRV/GKV gestellt |
| `Vorl. Erhalten` | Vorläufige Bescheinigung liegt vor |
| `Genehmigt` | Offizielle A1-Bescheinigung liegt vor |

---

## Technischer Stack

| Komponente | Version |
|---|---|
| .NET | 10.0 (Windows) |
| UI-Framework | WinForms |
| Datenbank | Oracle (SIVAS-Schema) |
| Oracle-Treiber | Devart.Data.Oracle 11.1.100 |
| Konfiguration | Microsoft.Extensions.Configuration.Json |
| Zieldatenbank | `SIVAS.SL_A1_ANTRAG_TAB` |

---

## Voraussetzungen

- Windows 10/11 (64-bit)
- .NET 10 Runtime oder SDK
- Netzwerkzugang zum Oracle-Server (`10.10.10.36`, Port 1521)
- Devart-Lizenz (Direct-Mode, kein Oracle Client erforderlich)
- Zugang zum SIVAS-Schema (`sivas`-User)

---

## Installation & Konfiguration

### 1. Repository klonen

```bash
git clone git@github.com:DonHaasDev/A1Antrag.git
cd A1Antrag
```

### 2. Lokale Konfiguration anlegen

```bash
cp A1Antrag/appsettings.local.json.example A1Antrag/appsettings.local.json
```

Datei `appsettings.local.json` anpassen:

```json
{
  "ConnectionStrings": {
    "Oracle": "User Id=sivas;Password=DEIN_PASSWORT;Server=10.10.10.36;Direct=True;Sid=LINUX"
  }
}
```

> `appsettings.local.json` ist in `.gitignore` und wird **nie** ins Repository committed.

### 3. NuGet-Pakete wiederherstellen

```bash
dotnet restore
```

### 4. Bauen

```bash
dotnet build A1Antrag/A1Antrag.csproj -c Release
```

### 5. Starten

```bash
dotnet run --project A1Antrag/A1Antrag.csproj
```

Oder die fertige `.exe` aus `A1Antrag/bin/Release/net10.0-windows/` direkt starten.

---

## Bedienung

### Hauptfenster

| Aktion | Beschreibung | Kürzel |
|---|---|---|
| **Neu** | Neuen A1-Antrag anlegen | Strg+N |
| **Bearbeiten** | Markierten Datensatz öffnen | F2 / Doppelklick |
| **Löschen** | Markierten Datensatz löschen | – |
| **Beantragen** | Status auf „Beantragt" setzen | – |
| **Genehmigen** | Status auf „Genehmigt" setzen | – |
| **Vorl. Erhebung** | Status auf „Vorl. Erhalten" setzen | – |
| **Aktualisieren** | Daten neu laden | F5 |

### Zeilenfarben

| Farbe | Status |
|---|---|
| Grün | Genehmigt |
| Gelb | Beantragt |
| Blau | Vorl. Erhalten |
| Weiß | Angelegt |

### Erfassungsformular

Das Erfassungsformular ist in drei Bereiche gegliedert:

- **Mitarbeiter**: Personalnummer, Nachname, Vorname
- **Einsatzzeitraum**: Von-Datum, Bis-Datum
- **Einsatzort / Auftraggeber**: Kundennummer, Firma, Ansprechpartner, Adresse, Land

---

## Datenbankstruktur

**Tabelle:** `SIVAS.SL_A1_ANTRAG_TAB`

| Spalte | Typ | Beschreibung |
|---|---|---|
| `LFDNR` | NUMBER | Laufende Nummer (Primärschlüssel, auto-increment via MAX+1) |
| `PERS_NR` | NUMBER | Personalnummer des Mitarbeiters |
| `FAM_NAME` | VARCHAR2(20) | Nachname (Großbuchstaben) |
| `NAME_VORNAME` | VARCHAR2(20) | Vorname |
| `VON` | DATE | Beginn des Auslandseinsatzes |
| `BIS` | DATE | Ende des Auslandseinsatzes |
| `KDNR` | NUMBER | Kundennummer des Auftraggebers |
| `FIRMA` | VARCHAR2(200) | Firmenname des Einsatzorts |
| `ANSPRECH_NAME` | VARCHAR2(200) | Ansprechpartner Nachname |
| `ANSPRECH_VORNAME` | VARCHAR2(200) | Ansprechpartner Vorname |
| `STRASSE` | VARCHAR2(200) | Straße des Einsatzorts |
| `PLZ` | VARCHAR2(12) | Postleitzahl |
| `ORT` | VARCHAR2(100) | Ort |
| `LAND` | VARCHAR2(30) | Land |
| `STATUS` | VARCHAR2(200) | Aktueller Status des Antrags |
| `BEANTRAGT_JN` | VARCHAR2(1) | J = Antrag gestellt |
| `BEANTRAGT_AM` | DATE | Datum der Antragstellung |
| `BEANTRAGT_VON` | VARCHAR2(200) | OS-Benutzer der Antragstellung |
| `GENEHMIGT_JN` | VARCHAR2(1) | J = Bescheinigung erhalten |
| `GENEHMIGT_AM` | DATE | Datum der Genehmigung |
| `GENEHMIGT_VON` | VARCHAR2(200) | OS-Benutzer der Genehmigung |
| `VORL_ERH_JN` | VARCHAR2(1) | J = Vorläufige Bescheinigung |
| `VORL_ERH_AM` | DATE | Datum vorläufige Erhebung |
| `VORL_ERH_VON` | VARCHAR2(200) | OS-Benutzer vorläufige Erhebung |
| `ANGELEGT_AM` | DATE | Anlagedatum |
| `ANGELEGT_VON` | VARCHAR2(200) | Angelegt von (OS-User) |
| `BEARBEITET_AM` | DATE | Zuletzt bearbeitet am |
| `BEARBEITET_VON` | VARCHAR2(200) | Zuletzt bearbeitet von |

---

## App-Tracking

Die Anwendung protokolliert beim Start automatisch über das Oracle-Package `SL_APP_TRACKING`:

- .NET-Version, App-Version, geladene DLLs → `SL_APP_INFO`
- OS-Benutzer, Rechnername, Startzeitpunkt → `SL_APP_START_LOG`

Das Tracking-Script (`oracle/tracking_setup.sql`) muss einmalig in der Datenbank ausgeführt werden (liegt im Projekt [ProDB_Aend](https://github.com/DonHaasDev/ProDB_Aend)).

---

## Hinweis: A1-Antrag online stellen

Die eigentliche Antragstellung erfolgt **extern** über:

- **Deutsche Rentenversicherung**: [www.deutsche-rentenversicherung.de](https://www.deutsche-rentenversicherung.de) → Arbeitgeber → A1-Bescheinigung
- **Krankenkasse**: Bei GKV-versicherten Mitarbeitern über die zuständige Krankenkasse

Nach erfolgreicher Antragstellung den Datensatz in der Anwendung mit **„Beantragen"** auf den entsprechenden Status setzen.

---

## Projektstruktur

```
A1Antrag/
├── A1Antrag.sln
├── README.md
├── .gitignore
└── A1Antrag/
    ├── A1Antrag.csproj
    ├── Program.cs                      # Einstiegspunkt, lädt Konfiguration
    ├── AppTracking.cs                  # Oracle App-Tracking
    ├── Form1.cs / Form1.Designer.cs    # Hauptfenster (Grid + Toolbar)
    ├── A1AntragDetailForm.cs / *.Designer.cs  # Erfassungsformular
    ├── appsettings.json                # Konfigurationsstruktur
    ├── appsettings.local.json          # Lokale Credentials (gitignored)
    ├── appsettings.local.json.example  # Vorlage für Credentials
    └── Properties/
        └── AssemblyInfo.cs
```
