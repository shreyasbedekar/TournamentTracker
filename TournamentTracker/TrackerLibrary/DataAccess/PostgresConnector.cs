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
        private const string db = "Tournaments";
        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddress", model.EmailAddress);
                p.Add("@CellphoneNumber", model.CellphoneNumber);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("SELECT spPeople_Insert(@FirstName, @LastName, @EmailAddress, @CellphoneNumber);", p);
                model.Id = p.Get<int>("@id");
                return model;
            }
        }

        // TODO - Make the CreatePrize method actually save to the database
        public PrizeModel CreatePrize(PrizeModel model)
        {
           using (IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
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

        public TeamModel CreateTeam(TeamModel model)
        {
            using(IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("SELECT spTeams_Insert(@TeamName);", p);
                model.Id = p.Get<int>("@id");
                foreach (PersonModel tm in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamId", model.Id);
                    p.Add("@PersonId", tm.Id);
                    connection.Execute("SELECT spTeamMembers_Insert(@TeamId, @PersonId);", p);
                }
                return model;
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            using(IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
            {
                return connection.Query<PersonModel>("SELECT * FROM people").ToList();
            }
        }
    }
}
