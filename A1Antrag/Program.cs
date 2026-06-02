using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.local.json", optional: true)
    .Build();

string connectionString = config.GetConnectionString("Oracle")
    ?? throw new InvalidOperationException("ConnectionString 'Oracle' nicht gefunden. Bitte appsettings.local.json prüfen.");

ApplicationConfiguration.Initialize();
Application.Run(new A1Antrag.Form1(connectionString));
