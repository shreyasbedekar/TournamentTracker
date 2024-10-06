using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();
        public static void InitializeConnections(bool database, bool textFiles)
        {
            if (database)
            {
                // TODO - Set up the SQL Connector properly
                PostgresConnector postgres = new PostgresConnector();
                Connections.Add(postgres);
            }
            if (textFiles)
            {
                // TODO - Create the Text Connection
                TextConnection text = new TextConnection();
                Connections.Add(text);
            }
        }
    }
}
