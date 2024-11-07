using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;
using TrackerLibrary.DataAccess.TextHelpers;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public const string PrizesFile = "PrizeModels.csv";
        public const string PeopleFile = "PersonModels.csv";
        public const string TeamFile = "TeamModels.csv";
        public const string TournamentFile = "TournamentModels.csv";
        public const string MatchupFile = "MatchupModels.csv";
        public const string MatchupEntryFile = "MatchEntryModels.csv";
        public static IDataConnection Connection { get; private set; }
        private static IConfiguration _config;
        public static void InitializeConnections(IConfiguration config, DatabaseType db)
        {
            _config = config;
            TextConnectorProcessor.Initialize(config);
            if (db == DatabaseType.Postgres)
            {
                PostgresConnector postgres = new PostgresConnector();
                Connection =postgres;
            }
            else if (db == DatabaseType.TextFile)
            {
                TextConnector text = new TextConnector();
                Connection = text;
            }
        }
        public static string CnnString(string name)
        {
            return _config.GetConnectionString(name);
        }

        public static string AppKeyLookup(string key)
        {
            return _config[key];
        }
    }
}
