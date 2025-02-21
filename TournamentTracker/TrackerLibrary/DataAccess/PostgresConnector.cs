﻿using Dapper;
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
        public void CreatePerson(PersonModel model)
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
            }
        }

        // TODO - Make the CreatePrize method actually save to the database
        public void CreatePrize(PrizeModel model)
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
            }
        }

        public void CreateTeam(TeamModel model)
        {
            using (IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
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
            }
        }

        public void CreateTournament(TournamentModel model)
        {
            using (IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
            {
                SaveTournament(connection, model);
                SaveTournamentPrizes(connection, model);
                SaveTournamentEntries(connection, model);
                SaveTournamentRounds(connection, model);
                TournamentLogic.UpdateTournamentResults(model);
            }
        }

        private void SaveTournamentRounds(IDbConnection connection, TournamentModel model)
        {
            foreach (RoundModel round in model.Rounds)
            {
                foreach (MatchupModel matchup in round.Matchups)
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
        private void SaveTournament(IDbConnection connection, TournamentModel model)
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
            using (IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
            {
                return connection.Query<PersonModel>("SELECT * FROM people").ToList();
            }
        }

        public List<TeamModel> GetTeam_All()
        {
            using (IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
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

        public List<TournamentModel> GetTournament_All()
        {
            List<TournamentModel> output;

            using (IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<TournamentModel>("SELECT * FROM spTournaments_GetAll()").DistinctBy(t=>t.Id).ToList();

                foreach (TournamentModel t in output)
                {

                    t.Prizes = connection.Query<PrizeModel>("SELECT * FROM spPrizes_GetByTournament(@TournamentId)", new { TournamentId = t.Id }).ToList();

                    t.EnteredTeams = connection.Query<TeamModel>("SELECT * FROM spTeams_GetByTournament(@TournamentId)", new { TournamentId = t.Id }).ToList();
                    foreach (TeamModel tm in t.EnteredTeams)
                    {
                        tm.TeamMembers = connection.Query<PersonModel>(
                            "SELECT * FROM teammembers tm INNER JOIN people p ON tm.PersonId = p.Id WHERE tm.TeamId = @TeamId",
                            new { TeamId = tm.Id }
                        ).ToList();
                    }
                    List<MatchupModel> matchups = connection.Query<MatchupModel>("SELECT * FROM spMatchups_GetByTournament(@TournamentId)", new { TournamentId = t.Id }).ToList();
                    foreach(var matchup in matchups)
                    {
                        Console.WriteLine($"Matchup id: {matchup.Id} Round: {matchup.MatchupRound}, Winner: {matchup.WinnerId}");
                    }
                    foreach (MatchupModel m in matchups)
                    {
                        m.Entries = connection.Query<MatchupEntryModel>("SELECT * FROM spMatchupEntries_GetByMatchup(@MatchupId)", new { MatchupId = m.Id }).ToList();
                        foreach (var entry in m.Entries)
                        {
                            Console.WriteLine($"Entry ID: {entry.Id}, TeamCompetingId: {entry.TeamCompetingId}, ParentMatchupId: {entry.ParentMatchupId}");
                        }
                        // Set Winner and TeamCompeting properties for entries
                        List<TeamModel> allTeams = GetTeam_All();
                        if (m.WinnerId > 0)
                        {
                            m.Winner = allTeams.FirstOrDefault(x => x.Id == m.WinnerId);
                        }

                        foreach (MatchupEntryModel me in m.Entries)
                        {
                            if (me.TeamCompetingId > 0)
                            {
                                me.TeamCompeting = allTeams.FirstOrDefault(x => x.Id == me.TeamCompetingId);
                            }

                            if (me.ParentMatchupId > 0)
                            {
                                me.ParentMatchup = matchups.FirstOrDefault(x => x.Id == me.ParentMatchupId);
                            }
                        }
                    }
                    t.Rounds = new List<RoundModel>();
                    var groupedMatchups = matchups.GroupBy(x => x.MatchupRound).ToList();
                    foreach (var group in groupedMatchups)
                    {
                        t.Rounds.Add(new RoundModel { Matchups = group.ToList() });
                    }
                }
            }
            return output;
        }

        public void UpdateMatchup(MatchupModel model)
        {
            var p = new DynamicParameters();
            using (IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
            {
                if (model.Winner != null)
                {
                    p.Add("@id", model.Id);
                    p.Add("@WinnerId", model.Winner.Id);
                    connection.Execute("SELECT spMatchups_Update(@id,@WinnerId);", p);
                }
                foreach (MatchupEntryModel entry in model.Entries)
                {
                    if (entry.TeamCompeting != null)
                    {
                        p = new DynamicParameters();
                        p.Add("@Id", entry.Id);
                        p.Add("@TeamCompetingId", entry.TeamCompeting.Id);
                        p.Add("@Score", entry.Score);
                        connection.Execute("SELECT spMatchupEntries_Update(@Id, @TeamCompetingId, @Score);", p);
                    }
                }
            }
        }

        public void CompleteTournament(TournamentModel model)
        {
           using(IDbConnection connection = new NpgsqlConnection(GlobalConfig.CnnString(db)))
           {
                var p = new DynamicParameters();
                p.Add("@id", model.Id);
                connection.Execute("SELECT spTournaments_Complete(@id);", p);
           }
        }
    }
}
