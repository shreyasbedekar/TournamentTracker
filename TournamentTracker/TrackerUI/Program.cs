using Microsoft.Extensions.Configuration;
using TrackerLibrary;

namespace TrackerUI
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
            ApplicationConfiguration.Initialize();

            // Load the configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the current directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // Load appsettings.json
                .Build();

            // Initialize the database connections
            GlobalConfig.InitializeConnections(config,DatabaseType.Postgres);
            Application.Run(new CreatePrizeForm());

            //Application.Run(new TournamentDashboardForm());
        }
    }
}