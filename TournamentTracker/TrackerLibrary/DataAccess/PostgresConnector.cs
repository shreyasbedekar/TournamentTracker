using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class PostgresConnector : IDataConnection
    {
        // TODO - Make the CreatePrize method actually save to the database
        public PrizeModel CreatePrize(PrizeModel model)
        {
           using (IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString("Tournaments")))
           {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = connection.Query<int>("SELECT spPrizes_Insert(@PlaceNumber, @PlaceName, @PrizeAmount, @PrizePercentage);", p);
                model.Id = result.First();
                return model;
            }
        }
    }
}
