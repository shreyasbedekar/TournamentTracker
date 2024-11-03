using System.ComponentModel;
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

            foreach (var matchup in matchupsToAdd)
            {
                selectedMatchups.Add(matchup);
            }
            LoadMatchup(selectedMatchups.FirstOrDefault());
        }


        private void LoadMatchup(MatchupModel m)
        {
            if (m == null) return;

            for (int i = 0; i < m.Entries.Count; i++)
            {
                var entry = m.Entries[i];
                if (entry.TeamCompeting != null)
                {
                    if (i == 0)
                    {
                        TeamOneName.Text = entry.TeamCompeting.TeamName;
                        teamOneScoreValue.Text = entry.Score.ToString();
                    }
                    else if (i == 1)
                    {
                        teamTwoName.Text = entry.TeamCompeting.TeamName;
                        teamTwoScoreValue.Text = entry.Score.ToString();
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        TeamOneName.Text = "Team not yet determined";
                        teamOneScoreValue.Text = "";
                    }
                    else if (i == 1)
                    {
                        teamTwoName.Text = "Team not yet determined";
                        teamTwoScoreValue.Text = "";
                    }
                }
            }
        }

        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchup( (MatchupModel)matchupListBox.SelectedItem);
        }
    }
}
