﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; }
        private static IConfiguration _config;
        public static void InitializeConnections(IConfiguration config, DatabaseType db)
        {
            _config = config;
            if (db == DatabaseType.Postgres)
            {
                // TODO - Set up the SQL Connector properly
                PostgresConnector postgres = new PostgresConnector();
                Connection =postgres;
            }
            else if (db == DatabaseType.TextFile)
            {
                // TODO - Create the Text Connection
                TextConnector text = new TextConnector();
                Connection = text;
            }
        }
        public static string CnnString(string name)
        {
            return _config.GetConnectionString(name);
        }
    }
}
