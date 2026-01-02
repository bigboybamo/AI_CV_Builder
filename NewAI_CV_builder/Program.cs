using DotNetEnv;
using Serilog;

namespace NewAI_CV_builder
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            Env.Load();

            var appName = "Cv_BuiderWinforms"; 

            var logFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName,
                "Logs"
            );

            // Ensure the directory exists
            Directory.CreateDirectory(logFolder);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: Path.Combine(logFolder, "log-.txt"),
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}