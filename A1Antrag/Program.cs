using Microsoft.Extensions.Configuration;
using System;
using System.Windows.Forms;

namespace A1Antrag
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.local.json", optional: true)
                .Build();

            string connectionString = config.GetConnectionString("Oracle")
                ?? throw new InvalidOperationException("ConnectionString 'Oracle' nicht gefunden. Bitte appsettings.local.json prüfen.");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(connectionString));
        }
    }
}
