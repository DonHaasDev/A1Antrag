using A1Antrag;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;

WindowsFormsSettings.LoadApplicationSettings();
UserLookAndFeel.Default.SetSkinStyle("Office 2019 Colorful");

ApplicationConfiguration.Initialize();
Application.Run(new Form1(Config.Oracle.ConnectionString));
