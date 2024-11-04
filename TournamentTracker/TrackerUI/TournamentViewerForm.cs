using System.ComponentModel;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {
        private TournamentModel _tournamentModel;
        BindingList<int> rounds = new BindingList<int>();
        BindingList<MatchupModel> selectedMatchups = new BindingList<MatchupModel>();

        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();
            _tournamentModel = tournamentModel;
            WireUpLists();

            if (_tournamentModel.Rounds == null || !_tournamentModel.Rounds.Any())
            {
                MessageBox.Show("No rounds available in the tournament data.");
                return;
            }
            LoadFormData();
            LoadRounds();
        }

        private void LoadFormData()
        {
            tournamentName.Text = _tournamentModel.TournamentName;
        }

        private void WireUpLists()
        {
            roundDropDown.DataSource = rounds;

            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = "DisplayName";
        }

        private void LoadRounds()
        {
            rounds.Clear();

            for (int i = 0; i < _tournamentModel.Rounds.Count; i++)
            {
                if (_tournamentModel.Rounds[i].RoundNumber == 0)
                {
                    _tournamentModel.Rounds[i].RoundNumber = i + 1;
                }
                if (!rounds.Contains(_tournamentModel.Rounds[i].RoundNumber))
                {
                    rounds.Add(_tournamentModel.Rounds[i].RoundNumber);
                }
            }
            if (rounds.Count == 0)
            {
                MessageBox.Show("No valid rounds found.");
            }
            else
            {
                LoadMatchups((int)rounds[0]);
            }
        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)roundDropDown.SelectedItem);
        }

        private void LoadMatchups(int round)
        {
            if (roundDropDown.SelectedItem == null) return;

            int selectedRoundNumber = (int)roundDropDown.SelectedItem;
            RoundModel selectedRound = _tournamentModel.Rounds
                .FirstOrDefault(r => r.RoundNumber == selectedRoundNumber);

            selectedMatchups.Clear();

            if (selectedRound == null)
            {
                MessageBox.Show("No matchups found for selected round.");
                return;
            }

            var matchupsToAdd = selectedRound.Matchups;
            if (selectedRoundNumber == 1)
            {
                matchupsToAdd = selectedRound.Matchups
                    .Where(m => m.Entries.Count == 2 && m.Entries.All(e => e.TeamCompeting != null)).ToList();
            }
            if (unplayedOnlyCheckbox.Checked)
            {
                matchupsToAdd = matchupsToAdd.Where(m => m.Winner == null).ToList();
            }
            foreach (var matchup in matchupsToAdd)
            {
                selectedMatchups.Add(matchup);
            }
            LoadMatchup(selectedMatchups.FirstOrDefault());
            DisplayMatchupInfo();
        }

        private void DisplayMatchupInfo()
        {
            bool isVisible = selectedMatchups.Count > 0;
            TeamOneName.Visible = isVisible;
            teamOneScoreLabel.Visible = isVisible;
            teamOneScoreValue.Visible = isVisible;
            teamTwoName.Visible = isVisible;
            teamTwoScoreLabel.Visible = isVisible;
            teamTwoScoreValue.Visible = isVisible;
            versusLabel.Visible = isVisible;
            scoreButton.Visible = isVisible;
        }

        private void LoadMatchup(MatchupModel m)
        {
            if (m == null) return;

            TeamOneName.Text = "Team not yet determined";
            teamOneScoreValue.Text = "";
            teamTwoName.Text = "<Bye>";
            teamTwoScoreValue.Text = "0";

            if (m.Entries.Count > 0 && m.Entries[0].TeamCompeting != null)
            {
                TeamOneName.Text = m.Entries[0].TeamCompeting.TeamName;
                teamOneScoreValue.Text = m.Entries[0].Score.ToString();
            }

            if (m.Entries.Count > 1)
            {
                if (m.Entries[1].TeamCompeting != null)
                {
                    teamTwoName.Text = m.Entries[1].TeamCompeting.TeamName;
                    teamTwoScoreValue.Text = m.Entries[1].Score.ToString();
                }
                else
                {
                    teamTwoName.Text = "Team not yet determined";
                    teamTwoScoreValue.Text = "";
                }
            }
        }

        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchup((MatchupModel)matchupListBox.SelectedItem);
        }

        private void unplayedOnlyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)roundDropDown.SelectedItem);

        }

        private void scoreButton_Click(object sender, EventArgs e)
        {
            MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;
            double teamOneScore = 0;
            double teamTwoScore = 0;
            if (m.Entries.Count > 0 && m.Entries[0].TeamCompeting != null)
            {
                if (!double.TryParse(teamOneScoreValue.Text, out teamOneScore))
                {
                    MessageBox.Show("Please enter a valid score for team one.");
                    return;
                }
                m.Entries[0].Score = teamOneScore;
            }

            if (m.Entries.Count > 1 && m.Entries[1].TeamCompeting != null)
            {
                if (!double.TryParse(teamTwoScoreValue.Text, out teamTwoScore))
                {
                    MessageBox.Show("Please enter a valid score for team two.");
                    return;
                }
                m.Entries[1].Score = teamTwoScore;
            }

            if (teamOneScore > teamTwoScore)
            {
                m.Winner = m.Entries[0].TeamCompeting;
            }
            else if (teamTwoScore > teamOneScore)
            {
                m.Winner = m.Entries[1].TeamCompeting;
            }
            else
            {
                MessageBox.Show("We do not allow ties in this application.");
                return;
            }

            GlobalConfig.Connection.UpdateMatchup(m);
            foreach (var round in _tournamentModel.Rounds)
            {
                foreach (var matchup in round.Matchups)
                {
                    foreach (var entry in matchup.Entries)
                    {
                        if (entry.ParentMatchup != null && entry.ParentMatchup.Id == m.Id)
                        {
                            entry.TeamCompeting = m.Winner;
                            GlobalConfig.Connection.UpdateMatchup(matchup);
                        }
                    }
                }
            }

            LoadMatchups((int)roundDropDown.SelectedItem);
        }
    }
}
