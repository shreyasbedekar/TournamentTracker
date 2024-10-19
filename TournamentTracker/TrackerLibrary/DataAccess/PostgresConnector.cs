using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        public void CreateTournament(TournamentModel model)
        {
            using(IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
            {
                SaveTournament(connection, model);
                SaveTournamentPrizes(connection, model);
                SaveTournamentEntries(connection, model);
                SaveTournamentRounds(connection, model);
            }
        }

        private void SaveTournamentRounds(IDbConnection connection, TournamentModel model)
        {
            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", model.Id);
                    p.Add("@MatchupRound", matchup.MatchupRound);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("SELECT spMatchups_Insert(@TournamentId, @MatchupRound);", p);
                    matchup.Id = p.Get<int>("@id");

                    foreach (MatchupEntryModel entry in matchup.Entries)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", matchup.Id);

                        if (entry.ParentMatchup == null)
                        {
                            p.Add("@ParentMatchupId", null);
                        }
                        else
                        {
                            p.Add("@ParentMatchupId", entry.ParentMatchup.Id);
                        }

                        if (entry.TeamCompeting == null)
                        {
                            p.Add("@TeamCompetingId", null);
                        }
                        else
                        {
                            p.Add("@TeamCompetingId", entry.TeamCompeting.Id);
                        }

                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                        connection.Execute("SELECT spMatchupEntries_Insert(@MatchupId, @ParentMatchupId, @TeamCompetingId);", p);
                    }
                }
            }
        }
        private void SaveTournament(IDbConnection connection,TournamentModel model)
        {
                var p = new DynamicParameters();
                p.Add("@TournamentName", model.TournamentName);
                p.Add("@EntryFee", model.EntryFee);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("SELECT spTournaments_Insert(@TournamentName, @EntryFee);", p);
                model.Id = p.Get<int>("@id");
        }

        private void SaveTournamentPrizes(IDbConnection connection, TournamentModel model)
        {
            foreach (PrizeModel pm in model.Prizes)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@PrizeId", pm.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("SELECT spTournamentPrizes_Insert(@TournamentId, @PrizeId);", p);
            }
        }

        private void SaveTournamentEntries(IDbConnection connection, TournamentModel model)
        {
            foreach (TeamModel tm in model.EnteredTeams)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@TeamId", tm.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("SELECT spTournamentEntries_Insert(@TournamentId, @TeamId);", p);
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            using(IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
            {
                return connection.Query<PersonModel>("SELECT * FROM people").ToList();
            }
        }

        public List<TeamModel> GetTeam_All()
        {
            using(IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
            {
                var teams = connection.Query<TeamModel>("SELECT * FROM spTeam_GetAll()").ToList();
                foreach (TeamModel team in teams)
                {
                    var p = new DynamicParameters();
                    p.Add("@TeamId", team.Id);
                    team.TeamMembers = connection.Query<PersonModel>("SELECT * FROM teammembers tm INNER JOIN people p ON tm.PersonId = p.Id WHERE tm.TeamId = @TeamId", p).ToList();
                }
                return teams;
            }
        }
    }
}
