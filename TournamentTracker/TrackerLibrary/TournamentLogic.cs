using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(new RoundModel
            {
                Matchups = CreateFirstRound(byes, randomizedTeams)
            });

            CreateOtherRounds(model, rounds);
        }

        public static void UpdateTournamentResults(TournamentModel model)
        {
            int startingRound = model.Rounds.Max(r=>r.CheckCurrentRound(model));
            List<MatchupModel> toScore = new List<MatchupModel>();

            foreach (RoundModel round in model.Rounds)
            {
                foreach (MatchupModel rm in round.Matchups)
                {
                    if (rm.Winner == null && (rm.Entries.Any(x => x.Score != 0) || rm.Entries.Count == 1))
                    {
                        toScore.Add(rm);
                    }
                }
            }

            MarkWinnerInMatchups(toScore);

            AdvanceWinners(toScore, model);

            toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x));
            int endingRound = model.Rounds.Max(r => r.CheckCurrentRound(model));
            if (endingRound > startingRound)
            {
                RoundModel currentRound = model.Rounds.FirstOrDefault(r => r.CheckCurrentRound(model) == endingRound);
                if (currentRound != null)
                {
                    AlertUsersToNewRound(endingRound, currentRound);
                }
            }
        }

        public static void AlertUsersToNewRound(int currentRoundNumber, RoundModel model)
        {
            List<MatchupModel> currentRound = model.Matchups.Where(x => x.MatchupRound == currentRoundNumber).ToList();
            foreach (MatchupModel matchup in currentRound)
            {
                foreach (MatchupEntryModel me in matchup.Entries)
                {
                    foreach (PersonModel p in me.TeamCompeting.TeamMembers)
                    {
                        AlertPersonToNewRound(p, me.TeamCompeting.TeamName, matchup.Entries.FirstOrDefault(x => x.TeamCompetingId != me.TeamCompetingId));            
                    }
                }
            }
        }

        private static void AlertPersonToNewRound(PersonModel p, string teamName, MatchupEntryModel competitor)
        {

            if (string.IsNullOrEmpty(p.EmailAddress))
            {
                return;
            }
            string fromAddress = "";
            string to = "";
            string subject = "";
            StringBuilder body = new StringBuilder();
            if(competitor!= null) {
                subject = $"You have a new matchup with {competitor.TeamCompeting.TeamName}";
                body.AppendLine("<h1>Matchup</h1>");
                body.Append("<strong>Competitor: </strong>");
                body.AppendLine(competitor.TeamCompeting.TeamName);
                body.AppendLine();
                body.AppendLine();
                body.AppendLine("Have a great time!");
                body.AppendLine("~Tournament Tracker");
            }
            else
            {
                subject = "You have a bye week this round.";
                body.AppendLine("Enjoy your round off!");
                body.AppendLine("~Tournament Tracker");
            }
            to= p.EmailAddress;
            EmailLogic.SendEmail(to, subject, body.ToString());
        }

        private static int CheckCurrentRound(this RoundModel round,TournamentModel tournament)
        {
            int output = 1;

            foreach (MatchupModel matchup in round.Matchups)
            {
                if (matchup.Winner != null && matchup.Winner.Id != 0)
                {
                    output+=1;
                }
                else
                {
                    return output;
                }
            }
            CompleteTournament(tournament);
            return output-1;
        }

        private static void CompleteTournament(TournamentModel tournament)
        {
            if (tournament == null || tournament.Rounds == null || tournament.EnteredTeams == null || tournament.Prizes == null)
            {
                Console.WriteLine("Tournament or one of its properties is null.");
                return;
            }

            GlobalConfig.Connection.CompleteTournament(tournament);

            TeamModel winners = null;
            TeamModel runnerUp = null;

            if (tournament.Rounds.Any() && tournament.Rounds.Last().Matchups.Any())
            {
                winners = tournament.Rounds.Last().Matchups.First().Winner;
                if (winners != null)
                {
                    runnerUp = tournament.Rounds.Last().Matchups.First()
                                  .Entries.FirstOrDefault(x => x.TeamCompeting != winners)?.TeamCompeting;
                }
            }

            decimal winnerPrize = 0;
            decimal runnerUpPrize = 0;
            if (tournament.Prizes.Count > 0 && tournament.EntryFee > 0 && tournament.EnteredTeams.Count > 0)
            {
                decimal totalIncome = tournament.EnteredTeams.Count * tournament.EntryFee;

                PrizeModel firstPlacePrize = tournament.Prizes.FirstOrDefault(x => x.PlaceNumber == 1);
                PrizeModel secondPlacePrize = tournament.Prizes.FirstOrDefault(x => x.PlaceNumber == 2);
                if (firstPlacePrize != null)
                {
                    winnerPrize = firstPlacePrize.CalculatePrizePayout(totalIncome);
                }
                if (secondPlacePrize != null)
                {
                    runnerUpPrize = secondPlacePrize.CalculatePrizePayout(totalIncome);
                }
            }

            string to = "";
            string subject = winners != null
                ? $"In {tournament.TournamentName}, {winners.TeamName} has Won!"
                : $"In {tournament.TournamentName}, the tournament has not been completed.";
            StringBuilder body = new StringBuilder();
            body.AppendLine("<h1>We have a Winner</h1>");
            body.AppendLine("<p>Congratulations to our Winner</p>");
            body.AppendLine("<br/>");

            if (winners != null)
            {
                body.AppendLine($"<p>{winners.TeamName} will receive ${winnerPrize}</p>");
            }
            else
            {
                body.AppendLine("<p>The winner could not be determined.</p>");
            }

            if (runnerUp != null && runnerUpPrize > 0)
            {
                body.AppendLine($"<p>{runnerUp.TeamName} will receive ${runnerUpPrize}</p>");
            }
            body.AppendLine("<p>Thank you for a great Tournament!</p>");
            body.AppendLine("~Tournament Tracker");

            List<string> bcc = new List<string>();
            foreach (TeamModel t in tournament.EnteredTeams)
            {
                foreach (PersonModel p in t.TeamMembers)
                {
                    if (!string.IsNullOrEmpty(p.EmailAddress))
                    {
                        if (winners != null && t.TeamName != winners.TeamName)
                        {
                            bcc.Add(p.EmailAddress);
                        }
                        else
                        {
                            to = p.EmailAddress;
                        }
                    }
                }
            }

            try
            {
                EmailLogic.SendEmail(new List<string>(), bcc, subject, body.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }

            // Ensure CompleteTournament is called even if email fails
            tournament.CompleteTournament();
        }

        private static decimal CalculatePrizePayout(this PrizeModel prize, decimal totalIncome)
        {
            decimal output = 0;

            if (prize.PrizeAmount > 0)
            {
                output = prize.PrizeAmount;
            }
            else
            {
                output = Decimal.Multiply(totalIncome, Convert.ToDecimal(prize.PrizePercentage / 100));
            }

            return output;
        }

        private static void MarkWinnerInMatchups(List<MatchupModel> models)
        {
            string greaterWins = models.First().MatchupRound > 1 ? "higher" : "lower";

            foreach (MatchupModel m in models)
            {
                if (m.Entries.Count == 1)
                {
                    m.Winner = m.Entries[0].TeamCompeting;
                    continue;
                }

                if (greaterWins == "higher")
                {
                    if (m.Entries[0].Score > m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[1].Score > m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We do not allow ties in this application.");
                    }
                }
                else
                {
                    if (m.Entries[0].Score < m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[1].Score < m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We do not allow ties in this application.");
                    }
                }
            }
        }
        private static void AdvanceWinners(List<MatchupModel> models, TournamentModel tournament)
        {
            foreach (MatchupModel m in models)
            {
                foreach (RoundModel round in tournament.Rounds)
                {
                    foreach (MatchupModel rm in round.Matchups)
                    {
                        foreach (MatchupEntryModel me in rm.Entries)
                        {
                            if (me.ParentMatchup != null)
                            {
                                if (me.ParentMatchup.Id == m.Id)
                                {
                                    me.TeamCompeting = m.Winner;
                                    GlobalConfig.Connection.UpdateMatchup(rm);
                                }
                            }
                        }
                    }
                }
            }
        }
        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }

        private static int FindNumberOfRounds(int teamCount)
        {
            int val = 1;
            int counter = 2;

            while (counter < teamCount)
            {
                val += 1;
                counter *= 2;
            }

            return val;
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            return (int)Math.Pow(2, rounds) - numberOfTeams;
        }

        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel curr = new MatchupModel();

            foreach (TeamModel team in teams)
            {
                curr.Entries.Add(new MatchupEntryModel { TeamCompeting = team });

                if (byes > 0 || curr.Entries.Count > 1)
                {
                    curr.MatchupRound = 1;
                    output.Add(curr);
                    curr = new MatchupModel();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }

            return output;
        }

        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int round = 2;
            List<MatchupModel> previousRound = model.Rounds[0].Matchups;
            List<MatchupModel> currRound = new List<MatchupModel>();
            MatchupModel currMatchup = new MatchupModel();

            while(true) {
                foreach (MatchupModel match in previousRound)
                {
                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match });

                    if (currMatchup.Entries.Count > 1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new MatchupModel();
                    }
                }

                model.Rounds.Add(new RoundModel { Matchups = currRound});
                previousRound = currRound;

                if (previousRound.Count == 1)
                {
                    break;
                }

                currRound = new List<MatchupModel>();
                round += 1;
            }

        }

    }
}
