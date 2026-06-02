using Devart.Data.Oracle;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace A1Antrag
{
    internal static class AppTracking
    {
        private const string AppName = "A1Antrag";

        public static void Track(OracleConnection connection)
        {
            try
            {
                string dotnetVersion = RuntimeInformation.FrameworkDescription;
                string appVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";
                string packages = GetPackageList();

                RegisterApp(connection, dotnetVersion, appVersion, packages);
                LogStart(connection, dotnetVersion, appVersion);
            }
            catch (Exception ex)
            {
                Debug.Print($"AppTracking: {ex.Message}");
            }
        }

        private static void RegisterApp(OracleConnection connection, string dotnetVersion, string appVersion, string packages)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "call SL_APP_TRACKING.app_register(:app_name, :dotnet_version, :app_version, :packages)";
            cmd.Parameters.Add(new OracleParameter("app_name", AppName));
            cmd.Parameters.Add(new OracleParameter("dotnet_version", dotnetVersion));
            cmd.Parameters.Add(new OracleParameter("app_version", appVersion));
            cmd.Parameters.Add(new OracleParameter("packages", packages));
            cmd.ExecuteNonQuery();
        }

        private static void LogStart(OracleConnection connection, string dotnetVersion, string appVersion)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "call SL_APP_TRACKING.app_log_start(:app_name, :os_user, :machine_name, :dotnet_version, :app_version)";
            cmd.Parameters.Add(new OracleParameter("app_name", AppName));
            cmd.Parameters.Add(new OracleParameter("os_user", Environment.UserName));
            cmd.Parameters.Add(new OracleParameter("machine_name", Environment.MachineName));
            cmd.Parameters.Add(new OracleParameter("dotnet_version", dotnetVersion));
            cmd.Parameters.Add(new OracleParameter("app_version", appVersion));
            cmd.ExecuteNonQuery();
        }

        private static string GetPackageList()
        {
            try
            {
                var entries = Directory.GetFiles(AppContext.BaseDirectory, "*.dll")
                    .Select(f =>
                    {
                        try
                        {
                            var name = AssemblyName.GetAssemblyName(f);
                            return $"{name.Name} {name.Version}";
                        }
                        catch { return null; }
                    })
                    .Where(e => e != null)
                    .OrderBy(e => e);

                return string.Join(", ", entries);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
